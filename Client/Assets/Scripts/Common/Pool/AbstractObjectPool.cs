﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Common
{
    public abstract class AbstractObjectPool<T, U> where T : MonoBehaviour where U : ICollection<T>, new() 
    {
        public U PoolList { get; protected set; } = new U();

        /// <summary>
        /// オブジェクトプールからプール対象のオブジェクトを取得する条件
        /// </summary>
        public delegate bool ConditionToGetFromPool(T instance);

        /// <summary>
        /// インスタンスの作り方
        /// </summary>
        public delegate T InstantiateMethod();

        /// <summary>
        /// インスタンス生成時に行うオプション
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public delegate T InstantiateOption(T instance);

        /// <summary>
        /// プールから取得する条件
        /// 指定しなければ'!activeSelf'(非活性オブジェクト)
        /// </summary>
        public ConditionToGetFromPool IsGetPool = null;

        /// <summary>
        /// インスタンス生成時の作成方法
        /// </summary>
        /// <detail>
        /// * Instantiate
        /// * NGUITools.Add()
        ///  インスタンス化時に細かく条件を指定したい時用。
        ///  上記のように生成メソッドが違う場合や
        ///  生成時にQuaternion.identityに設定する、
        ///  ObjectのParentを指定する時などなど。
        ///  使用側でWrap出来るよう汎用性を持たせる。
        /// </detail>
        public InstantiateMethod CreateInstanceMethod = null;

        /// <summary>
        /// インスタンス生成直後に呼ばれるメソッド
        /// 生成直後のインスタンスに対して行いたい処理を定義。
        /// </summary>
        public InstantiateOption CreateInstanceOption = null;

        /// <summary>
        /// 空コンストラクタ隠蔽
        /// </summary>
        protected AbstractObjectPool() { }

        /// <summary>
        /// 空コンストラクタ
        /// プール取得条件:!gameObject.activeSelf
        /// 生成方式:Instantiate
        /// 生成時の処理:gameObject.SetActive(true)
        /// </summary>
        public  AbstractObjectPool(GameObject poolObject)
        {
            //取得条件
            IsGetPool = (instance) => { return !instance.gameObject.activeSelf; };
            //生成方法
            CreateInstanceMethod = () => { return GameObject.Instantiate(poolObject).GetComponent<T>(); };
            //生成時オプション
            CreateInstanceOption = (instance) =>
            {
                if (!instance.gameObject.activeSelf) instance.gameObject.SetActive(true);
                return instance;
            };
        }
        public AbstractObjectPool(T poolObject) : this(poolObject.gameObject) { }

        public AbstractObjectPool(GameObject poolObject, ConditionToGetFromPool isGetPool, InstantiateMethod createInstanceMethod, InstantiateOption createInstanceOption)
        {
            this.IsGetPool = isGetPool;
            this.CreateInstanceMethod = createInstanceMethod;
            this.CreateInstanceOption = createInstanceOption;
        }

        public T Get()
        {
            foreach (var current in PoolList)
            {
                if (IsGetPool(current))
                {
                    return current;
                }
            }
            var newInstance = Create();
            return newInstance;
        }

        private T Create()
        {
            var newInstance = CreateInstanceMethod();
            CreateInstanceOption?.Invoke(newInstance);
            return newInstance;
        }
    }

    /// <summary>
    /// List型プール
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPoolList<T> : AbstractObjectPool<T, List<T>>
        where T : MonoBehaviour
    {
        public ObjectPoolList(GameObject poolObject) : base(poolObject) { }
        public ObjectPoolList(T poolTarget) : base(poolTarget) { }
        public ObjectPoolList(GameObject poolObject, ConditionToGetFromPool isGetPool, InstantiateMethod createInstanceMethod, InstantiateOption createInstanceOption)
        {
            this.IsGetPool = isGetPool;
            this.CreateInstanceMethod = createInstanceMethod;
            this.CreateInstanceOption = createInstanceOption;
        }
    }

    /// <summary>
    /// LinkedList型プール
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPoolLinkedList<T> : AbstractObjectPool<T, LinkedList<T>>
        where T : MonoBehaviour
    {
        public ObjectPoolLinkedList(GameObject poolObject) : base(poolObject) { }
        public ObjectPoolLinkedList(T poolTarget) : base(poolTarget) { }
        public ObjectPoolLinkedList(GameObject poolObject, ConditionToGetFromPool isGetPool, InstantiateMethod createInstanceMethod, InstantiateOption createInstanceOption)
        {
            this.IsGetPool = isGetPool;
            this.CreateInstanceMethod = createInstanceMethod;
            this.CreateInstanceOption = createInstanceOption;
        }
    }
}