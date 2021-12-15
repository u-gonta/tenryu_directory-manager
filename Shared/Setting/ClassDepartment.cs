using System.IO;
using System.Runtime.Serialization;

namespace Shared
{
    namespace Setting
    {
        /// <summary>
        /// 部署のクラス
        /// </summary>
        [DataContract(Name = "Department")]
        public class ClassDepartment
        {
            /// <summary>
            /// 部署の種類
            /// </summary>
            public enum EnumCategory
            {
                Unknown,            // 不明
                Design,             // 設計
                Sales,              // 営業
                Assembly            // 組み立て
            }

            /// <summary>
            /// 部署の種類
            /// </summary>
            [DataMember(Order = 1, Name = "Category")]
            public EnumCategory Category { get; set; }

            /// <summary>
            /// ルートのディレクトリ
            /// </summary>
            [DataMember(Order = 2, Name = "RootDirectory")]
            public string RootDirectory { get; set; }

            /// <summary>
            /// 工番のディレクトリ名
            /// </summary>
            [DataMember(Order = 3, Name = "ProcessDirectory")]
            public string ProcessDirectory { get; set; }

            /// <summary>
            /// タイトル
            /// </summary>
            [DataMember(Order = 4, Name = "Title")]
            public string Title { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ClassDepartment()
            {
                // 初期化
                Initialize();
            }

            /// <summary>
            /// 初期化
            /// </summary>
            public void Initialize()
            {
                // ルートのディレクトリを確認
                if (this.RootDirectory == null)
                {
                    this.RootDirectory = "." + Path.DirectorySeparatorChar.ToString();
                }

                // 工番のディレクトリ名を確認
                if (this.ProcessDirectory == null)
                {
                    this.ProcessDirectory = "";
                }

                // タイトルを確認
                if (this.Title == null)
                {
                    this.Title = "";
                }
            }

            /// <summary>
            /// 部署のクラスをコピー
            /// </summary>
            /// <returns></returns>
            public ClassDepartment Clone()
            {
                var ret = new ClassDepartment();

                ret = (ClassDepartment)MemberwiseClone();

                return ret;
            }
        }
    }
}
