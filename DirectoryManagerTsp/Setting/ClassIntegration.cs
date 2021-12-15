using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace DirectoryManagerTsp
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
            /// 機種名のヘッダ
            /// </summary>
            public const string ModelHeader = "TSP-";

            /// <summary>
            /// 装置名のヘッダ
            /// </summary>
            public const string EquipmentHeader = "TS";

            /// <summary>
            /// テンプレートのルートディレクトリ
            /// </summary>
            [DataMember(Order = 1, Name = "TemplateRootDirectory")]
            public string TemplateRootDirectory { get; set; }

            /// <summary>
            /// テンプレートの工番ディレクトリ名
            /// </summary>
            [DataMember(Order = 2, Name = "TemplateProcessDirectory")]
            public string TemplateProcessDirectory { get; set; }

            /// <summary>
            /// 部署のクラス
            /// </summary>
            [DataMember(Order = 3, Name = "Department")]
            public List<Shared.Setting.ClassDepartment> Departments { get; set; }

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
                // テンプレートのルートディレクトリを確認
                if (this.TemplateRootDirectory == null)
                {
                    this.TemplateRootDirectory = "." + Path.DirectorySeparatorChar.ToString();
                }

                // テンプレートの工番ディレクトリ名を確認
                if (this.TemplateProcessDirectory == null)
                {
                    this.TemplateProcessDirectory = "";
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

                return ret;
            }
        }
    }
}
