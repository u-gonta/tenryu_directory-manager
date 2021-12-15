using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Shared
{
    namespace Generation
    {
        namespace Directory
        {
            /// <summary>
            /// ディレクトリ情報のテンプレート用クラス
            /// </summary>
            [DataContract(Name = "Directory")]
            public class ClassTemplate
            {
                /// <summary>
                /// コメント
                /// </summary>
                [DataMember(Order = 1, Name = "Comment")]
                public string Comment { get; set; }

                /// <summary>
                /// ディレクトリの情報クラス
                /// </summary>
                [DataMember(Order = 2, Name = "Directory")]
                public SortedDictionary<string, ClassInformation> Directories { get; set; }

                /// <summary>
                /// ファイルの情報クラス
                /// </summary>
                [DataMember(Order = 3, Name = "File")]
                public SortedDictionary<string, ClassInformation> Files { get; set; }

                /// <summary>
                /// コンストラクタ
                /// </summary>
                public ClassTemplate()
                {
                    // 初期化
                    Initialize();
                }

                /// <summary>
                /// 初期化
                /// </summary>
                public virtual void Initialize()
                {
                    // コメントを確認
                    if (Comment == null)
                    {
                        Comment = "";
                    }

                    // ディレクトリの情報クラスを確認
                    if (Directories == null)
                    {
                        Directories = new SortedDictionary<string, ClassInformation>();
                    }

                    // ファイルの情報クラスを確認
                    if (Files == null)
                    {
                        Files = new SortedDictionary<string, ClassInformation>();
                    }
                }

                /// <summary>
                /// 設定のクラスをコピー
                /// </summary>
                /// <returns></returns>
                public virtual object Clone()
                {
                    var ret = new ClassTemplate();

                    ret = (ClassTemplate)MemberwiseClone();

                    // ディレクトリの情報クラスをコピー
                    ret.Directories = new SortedDictionary<string, ClassInformation>(this.Directories);

                    // ファイルの情報クラスをコピー
                    ret.Files = new SortedDictionary<string, ClassInformation>(this.Files);

                    return ret;
                }
            }
        }
    }
}
