// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protocol.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Server.gRPC {
  /// <summary>
  /// RPCインターフェイス
  /// </summary>
  public static partial class Server
  {
    static readonly string __ServiceName = "gRPC.Server";

    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    static readonly grpc::Marshaller<global::Server.gRPC.C2S_Ping_Request> __Marshaller_gRPC_C2S_Ping_Request = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Server.gRPC.C2S_Ping_Request.Parser));
    static readonly grpc::Marshaller<global::Server.gRPC.S2C_Ping_Response> __Marshaller_gRPC_S2C_Ping_Response = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Server.gRPC.S2C_Ping_Response.Parser));

    static readonly grpc::Method<global::Server.gRPC.C2S_Ping_Request, global::Server.gRPC.S2C_Ping_Response> __Method_Ping = new grpc::Method<global::Server.gRPC.C2S_Ping_Request, global::Server.gRPC.S2C_Ping_Response>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Ping",
        __Marshaller_gRPC_C2S_Ping_Request,
        __Marshaller_gRPC_S2C_Ping_Response);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Server.gRPC.ProtocolReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of Server</summary>
    [grpc::BindServiceMethod(typeof(Server), "BindService")]
    public abstract partial class ServerBase
    {
      public virtual global::System.Threading.Tasks.Task<global::Server.gRPC.S2C_Ping_Response> Ping(global::Server.gRPC.C2S_Ping_Request request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for Server</summary>
    public partial class ServerClient : grpc::ClientBase<ServerClient>
    {
      /// <summary>Creates a new client for Server</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public ServerClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for Server that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public ServerClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected ServerClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected ServerClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::Server.gRPC.S2C_Ping_Response Ping(global::Server.gRPC.C2S_Ping_Request request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Ping(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Server.gRPC.S2C_Ping_Response Ping(global::Server.gRPC.C2S_Ping_Request request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Ping, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Server.gRPC.S2C_Ping_Response> PingAsync(global::Server.gRPC.C2S_Ping_Request request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return PingAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Server.gRPC.S2C_Ping_Response> PingAsync(global::Server.gRPC.C2S_Ping_Request request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Ping, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override ServerClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new ServerClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(ServerBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_Ping, serviceImpl.Ping).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, ServerBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_Ping, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Server.gRPC.C2S_Ping_Request, global::Server.gRPC.S2C_Ping_Response>(serviceImpl.Ping));
    }

  }
}
#endregion