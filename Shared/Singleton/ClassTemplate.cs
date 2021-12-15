namespace Shared
{
    namespace Singleton
    {
        /// <summary>
        /// シングルトンのテンプレート用クラス
        /// </summary>
        public class ClassTemplate<T> where T : class, new()
        {
            /// <summary>
            /// インスタンス変数への代入が完了するまで、アクセスできなくなるジェネリックなインスタンス
            /// </summary>
            private static volatile T _instance;

            /// <summary>
            /// インスタンス変数をロックするためのインスタンス
            /// </summary>
            protected static object _async = new object();

            /// <summary>
            /// ジェネリックなインスタンス
            /// </summary>
            public static T Instance
            {
                get
                {
                    // ダブルチェック ロッキング アプローチ
                    if (_instance == null)
                    {
                        // syncインスタンスをロックし、この型そのものをロックしないことで、デッドロックの発生を回避
                        lock (_async)
                        {
                            // インスタンスを確認
                            if (_instance == null)
                            {
                                // インスタンスを作成
                                _instance = new T();
                            }
                        }
                    }

                    return _instance;
                }
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            protected ClassTemplate()
            {

            }
        }
    }
}
