using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using Grpc.Core;
using Server.gRPC;
using Grpc;
using System.Threading;
using System.Threading.Tasks;
using Model.Chat;

/*
 * Taskで実行した場合終了時にshutdownしないとEditor上でも動き続ける
 */

namespace Sample
{
    public class ClientSample : MonoBehaviour
    {
        public static ClientSample Instance = new ClientSample();
        [SerializeField] uGUI.Chat.UIChatWindow chatWindow;
        Action S2C_Recive = null;
        Channel channel;
        Unary.UnaryClient unaryClient;
        BidirectionalStreaming.BidirectionalStreamingClient bidirectionalStreamingClient;
        public static SynchronizationContext context;
        SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        Task duplexChatReciveTask = null;
        uint cnt = 0;
        public string IPAddress { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 1122;

        // Start is called before the first frame update
        void Start()
        {
            context = SynchronizationContext.Current;
            Connect();
        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log($"state:{(channel != null ? channel.State.ToString() : "null")}");
            S2C_Recive?.Invoke();
            if (Input.GetKeyDown(KeyCode.B))
            {
                Debug.Log("「B」");
                cnt++;
                DuplexChatSend request = new DuplexChatSend { UserID = 0, Message = $"Count={cnt}" };
                ChatRequests.Add(request);
                ChatModel.Instance.AddRequestSendMessage(request);
            }
        }

        List<DuplexChatSend> ChatRequests = new List<DuplexChatSend>();
        /// <summary>
        /// 
        /// </summary>
        void OnUpdate()
        {
            OnUpdateChat().Wait(1);
        }

        void OnUpdatePing()
        {
            if (unaryClient != null && channel.State != ChannelState.TransientFailure)
            {
                try
                {
                    var ping = unaryClient.Ping(new C2S_Ping_Request());
                    if (ping != null)
                    {
                        Debug.Log($"ping:{ping.Ping }");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"<color=red>Ping</color>:{e.GetType()}\n{e.Message}\n{e.StackTrace}");
                    throw;
                }
            }
        }
        
        async Task OnUpdateChat()
        {
            if (bidirectionalStreamingClient == null || channel.State == ChannelState.TransientFailure)
            {
                return;
            }
            try
            {
                using (var call = bidirectionalStreamingClient.DuplexChat())
                {
                    //受信
                    duplexChatReciveTask = Task.Run(async () =>
                    {
                    await semaphore.WaitAsync();
                    try
                    {
                            while (await call.ResponseStream.MoveNext())
                            {
                                var current = call.ResponseStream.Current;

                                context.Post(_ =>
                                {
                                    ChatModel.Instance.S2C_Receive_Duplex_Chat(current);
                                }, null);
                                #region MVCクラス無し版
#if false
                                // 新規のメッセージだけを選別してリストに追加する
                                if (!ChatModel.Instance.Messages.Any(message => message.Hash == current.Hash))
                                {
                                    ChatModel.Instance.Messages.Add(current);
                                    context.Post(_ =>
                                    {
                                        chatWindow.OnReceiveChat(current);
                                    }, null);
                                }
#endif
                                #endregion
                            }
                            await call.ResponseHeadersAsync;
                        }
                        catch (Exception e)
                        {
                            context.Post(_ =>
                            {
                                Debug.LogError("Receiveタスク例外:" + e.GetType() + "\n" + e.Message);
                            }, null);
                            throw;
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    });

                    //送信
                    #region 直書き
#if false
                    //int i = 0;
                    //while (i < ChatRequests.Count)
                    //{
                    //    var request = ChatRequests[i];
                    //    //Debug.Log($"Send\n{request.UserID}:{request.Message}");

                    //    await call.RequestStream.WriteAsync(request);
                    //    i++;
                    //}
                    //ChatRequests.Clear();
                    await call.RequestStream.CompleteAsync();
                    await duplexChatReciveTask;
#endif
                    #endregion
                    await ChatModel.Instance.C2S_Send_Duplex_Chat(call, duplexChatReciveTask);
                }

            }
            catch (Exception e)
            {
                Debug.LogError($"<color=red>Chat</color>:{e.GetType()}\n{e.Message}\n{e.StackTrace}");
                duplexChatReciveTask.Dispose();
                Disconnect();
                throw;
            }
            finally
            {
            }
        }

        private void Connect()
        {
            try
            {
                channel = new Channel(IPAddress, Port, ChannelCredentials.Insecure
                    //, new List<ChannelOption>
                    //{
                    //    new ChannelOption(ChannelOptions.MaxReceiveMessageLength, 8388608/2), 
                    //}
                    );
                bidirectionalStreamingClient = new BidirectionalStreaming.BidirectionalStreamingClient(channel);
                unaryClient = new Unary.UnaryClient(channel);
                S2C_Recive = OnUpdate;
            }
            catch (Exception e)
            {
                Debug.LogError($"<color=red>Connect</color>:{e.GetType()}\n{e.Message}\n{e.StackTrace}");
                throw;
            }

        }

        private void Disconnect()
        {
            Debug.Log("<color=green>Disconnect</color>");
            channel.ShutdownAsync().Dispose();
            duplexChatReciveTask.Dispose();
        }
    }
}