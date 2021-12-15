using System.Configuration;

namespace DirectoryManagerTspOption
{
    namespace Setting
    {
        /// <summary>
        /// ローカルの設定
        /// </summary>
        public class ClassLocal : ApplicationSettingsBase
        {
            /// <summary>
            /// アカウント名
            /// </summary>
            private const string AccountKey = "Account";

            /// <summary>
            /// アカウント名
            /// </summary>
            [UserScopedSetting()]
            public string Account
            {
                get
                {
                    var ret = "";

                    ret = (string)this[AccountKey];

                    return ret;
                }
                set
                {
                    this[AccountKey] = value;
                }
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ClassLocal()
            {
                // 初期化
                Initialize();
            }

            /// <summary>
            /// 初期化
            /// </summary>
            public void Initialize()
            {
                // アカウント名を確認
                if (this[AccountKey] == null)
                {
                    this[AccountKey] = "";
                }
            }
        }
    }
}
