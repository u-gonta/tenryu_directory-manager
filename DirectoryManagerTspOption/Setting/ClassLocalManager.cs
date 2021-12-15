namespace DirectoryManagerTspOption
{
    namespace Setting
    {
        /// <summary>
        /// ローカルの設定クラスを管理するクラス
        /// ※シングルトンのテンプレート用クラスから継承
        /// </summary>
        public class ClassLocalManager : Shared.Singleton.ClassTemplate<ClassLocalManager>
        {
            /// <summary>
            /// ローカルの設定クラス
            /// </summary>
            private ClassLocal _local = new ClassLocal();

            /// <summary>
            /// アカウント名
            /// </summary>
            public string Account
            {
                get
                {
                    var ret = "";

                    ret = _local.Account;

                    return ret;
                }
                set
                {
                    _local.Account = value;
                }
            }

            /// <summary>
            /// 設定を保存
            /// </summary>
            public void Save()
            {
                // ファイルに保存
                _local.Save();
            }
        }
    }
}
