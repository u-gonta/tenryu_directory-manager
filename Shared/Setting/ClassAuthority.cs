using System.Runtime.Serialization;

namespace Shared
{
    namespace Setting
    {
        /// <summary>
        /// 権限のクラス
        /// </summary>
        [DataContract(Name = "Authority")]
        public class ClassAuthority
        {
            /// <summary>
            /// 権限の種類
            /// </summary>
            public enum EnumCategory
            {
                Unknown,                // 不明
                CreateModel,            // 機種の作成
                EditModelComment,       // 機種のコメント編集
                CreateEquipment,        // 装置の作成
                EditEquipmentComment,   // 装置のコメント編集
                CreateProcess,          // 工番の作成
                EditProcessComment,     // 工番のコメント編集
                CreateOption,           // オプションの作成
                EditOptionComment       // オプションのコメント編集
            }
        }
    }
}
