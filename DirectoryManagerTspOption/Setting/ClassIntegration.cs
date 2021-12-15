using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace DirectoryManagerTspOption
{
    namespace Setting
    {
        /// <summary>
        /// 設定を保持するクラス
        /// </summary>
        [DataContract(Name = "Integration")]
        public class ClassIntegration
        {
            /// <summary>
            /// テンプレートのディレクトリ
            /// </summary>
            [DataMember(Order = 1, Name = "TemplateDirectory")]
            public string TemplateDirectory { get; set; }

            /// <summary>
            /// 部署のクラス
            /// </summary>
            [DataMember(Order = 2, Name = "Department")]
            public List<Shared.Setting.ClassDepartment> Departments { get; set; }

            /// <summary>
            /// カテゴリ番号の名称
            /// </summary>
            [DataMember(Order = 3, Name = "Category")]
            public Dictionary<int, string> Categories { get; set; }

            /// <summary>
            /// 作業者の設定ファイル
            /// </summary>
            [DataMember(Order = 4, Name = "WorkerPath")]
            public string WorkerPath { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ClassIntegration()
            {
                // 初期化
                Initialize();
            }

            /// <summary>
            /// デシリアライズ
            /// </summary>
            /// <param name="context"></param>
            [OnDeserialized]
            internal void OnDeserializedMethod(StreamingContext context)
            {
                // 初期化
                Initialize();
            }

            /// <summary>
            /// 初期化
            /// </summary>
            public void Initialize()
            {
                // テンプレートのディレクトリを確認
                if (this.TemplateDirectory == null)
                {
                    this.TemplateDirectory = "." + Path.DirectorySeparatorChar.ToString();
                }

                // 部署のクラスを確認
                if (this.Departments == null)
                {
                    this.Departments = new List<Shared.Setting.ClassDepartment>();
                }
                foreach (var department in this.Departments)
                {
                    // 初期化
                    department.Initialize();
                }

                // カテゴリ番号を確認
                if (this.Categories == null)
                {
                    this.Categories = new Dictionary<int, string>();
                }

                // 作業者の設定ファイル
                if (WorkerPath == null)
                {
                    WorkerPath = "";
                }
            }

            /// <summary>
            /// 設定のクラスをコピー
            /// </summary>
            /// <returns></returns>
            public ClassIntegration Clone()
            {
                var ret = new ClassIntegration();

                ret = (ClassIntegration)MemberwiseClone();

                // 部署のクラスをコピー
                ret.Departments = new List<Shared.Setting.ClassDepartment>(this.Departments);

                // カテゴリ番号をコピー
                ret.Categories = new Dictionary<int, string>(this.Categories);

                return ret;
            }
        }
    }
}
