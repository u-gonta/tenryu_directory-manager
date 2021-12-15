using System.Runtime.Serialization;

namespace DirectoryManagerTsp
{
    namespace Generation
    {
        /// <summary>
        /// ディレクトリの情報クラス
        /// ※ディレクトリ情報のテンプレート用クラスから継承
        /// </summary>
        [DataContract(Name = "Directory")]
        class ClassDirectory : Shared.Generation.Directory.ClassTemplate
        {
            /// <summary>
            /// 派生元の機種名
            /// </summary>
            [DataMember(Order = 11, Name = "DerivationModel")]
            public string DerivationModel { get; set; }

            /// <summary>
            /// 派生元の装置名
            /// </summary>
            [DataMember(Order = 12, Name = "DerivationEquipment")]
            public string DerivationEquipment { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ClassDirectory() : base()
            {
                // 初期化
                Initialize();
            }

            /// <summary>
            /// 初期化
            /// </summary>
            public override void Initialize()
            {
                // 既定の関数
                base.Initialize();

                // 派生元の機種名を確認
                if (DerivationModel == null)
                {
                    DerivationModel = "";
                }

                // 派生元の装置名を確認
                if (DerivationEquipment == null)
                {
                    DerivationEquipment = "";
                }
            }

            /// <summary>
            /// 設定のクラスをコピー
            /// </summary>
            /// <returns></returns>
            public override object Clone()
            {
                var ret = new ClassDirectory();

                // 既定の関数
                ret = (ClassDirectory)base.Clone();

                ret = (ClassDirectory)MemberwiseClone();

                return ret;
            }
        }
    }
}
