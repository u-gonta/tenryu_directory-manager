using System.Runtime.Serialization;

namespace Shared
{
    namespace Generation
    {
        /// <summary>
        /// 生成する種類
        /// </summary>
        public enum EnumCategory
        {
            Invalid,        // 無効
            Empty,          // 空
            Copy,           // コピー
            Link            // リンク
        }

        /// <summary>
        /// 情報クラス
        /// </summary>
        [DataContract(Name = "Information")]
        public class ClassInformation
        {
            /// <summary>
            /// 生成する種類
            /// </summary>
            [DataMember(Order = 1, Name = "Category")]
            public EnumCategory Category { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ClassInformation()
            {
                Category = EnumCategory.Invalid;

                // 初期化
                Initialize();
            }

            /// <summary>
            /// 初期化
            /// </summary>
            public void Initialize()
            {

            }

            /// <summary>
            /// 設定のクラスをコピー
            /// </summary>
            /// <returns></returns>
            public ClassInformation Clone()
            {
                ClassInformation ret = new ClassInformation();

                ret = (ClassInformation)MemberwiseClone();

                return ret;
            }
        }
    }
}
