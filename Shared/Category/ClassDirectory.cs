namespace Shared
{
    namespace Category
    {
        /// <summary>
        /// ディレクトリの分類クラス
        /// </summary>
        public class ClassDirectory
        {
            /// <summary>
            /// ディレクトリのパス
            /// </summary>
            public string Path { get; set; }

            /// <summary>
            /// 名称
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ClassDirectory()
            {
                // 初期化
                Initialize();
            }

            /// <summary>
            /// 初期化
            /// </summary>
            public void Initialize()
            {
                // ディレクトリのパスを確認
                if (Path == null)
                {
                    Path = "";
                }

                // 名称を確認
                if (Title == null)
                {
                    Title = Shared.Select.ClassDefine.UnselectedName;
                }
            }

            /// <summary>
            /// ディレクトリのクラスをコピー
            /// </summary>
            /// <returns></returns>
            public ClassDirectory Clone()
            {
                var ret = new ClassDirectory();

                ret = (ClassDirectory)MemberwiseClone();

                return ret;
            }
        }
    }
}
