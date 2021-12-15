using System.Runtime.Serialization;

namespace Shared
{
    namespace Generation
    {
        namespace Option
        {
            /// <summary>
            /// オプションのディレクトリ情報クラス
            /// ※ディレクトリ情報のテンプレート用クラスから継承
            /// </summary>
            [DataContract(Name = "Directory")]
            public class ClassDirectory : Directory.ClassTemplate
            {
                /// <summary>
                /// 派生元のオプション名
                /// </summary>
                [DataMember(Order = 11, Name = "Derivation")]
                public string Derivation { get; set; }

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

                    // 派生元のオプション名を確認
                    if (Derivation == null)
                    {
                        Derivation = "";
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
}
