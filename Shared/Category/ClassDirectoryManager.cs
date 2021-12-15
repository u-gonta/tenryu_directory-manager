using System.Collections.Generic;

namespace Shared
{
    namespace Category
    {
        /// <summary>
        /// ディレクトリの分類クラスを管理するクラス
        /// </summary>
        public class ClassDirectoryManager
        {
            /// <summary>
            /// 分類名を保持するディレクトリのクラス
            /// </summary>
            public List<ClassDirectory> Directories = new List<ClassDirectory>();

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ClassDirectoryManager()
            {
                // 初期化
                Initialize();
            }

            /// <summary>
            /// 初期化
            /// </summary>
            public void Initialize()
            {
                // 分類名を保持するディレクトリのクラスを確認
                if (this.Directories == null)
                {
                    this.Directories = new List<ClassDirectory>();
                }
                foreach(var directory in this.Directories)
                {
                    directory.Initialize();
                }
            }

            /// <summary>
            /// 分類名のクラスをコピー
            /// </summary>
            /// <returns></returns>
            public ClassDirectoryManager Clone()
            {
                var ret = new ClassDirectoryManager();

                ret = (ClassDirectoryManager)MemberwiseClone();

                // 分類名を保持するディレクトリのクラスをコピー
                ret.Directories = new List<ClassDirectory>(this.Directories);

                return ret;
            }
        }
    }
}
