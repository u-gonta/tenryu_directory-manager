using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Shared
{
    namespace Setting
    {
        /// <summary>
        /// 作業者のクラス
        /// </summary>
        [DataContract(Name = "Worker")]
        public class ClassWorker
        {
            /// <summary>
            /// 部署の種類
            /// </summary>
            [DataMember(Order = 1, Name = "Department")]
            public ClassDepartment.EnumCategory Department { get; set; }

            /// <summary>
            /// 権限の種類
            /// </summary>
            [DataMember(Order = 2, Name = "Authoritys")]
            public Dictionary<ClassAuthority.EnumCategory, bool> Authoritys { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ClassWorker()
            {
                Department = ClassDepartment.EnumCategory.Unknown;

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
                // 権限の種類を確認
                if (Authoritys == null)
                {
                    Authoritys = new Dictionary<ClassAuthority.EnumCategory, bool>();
                }
            }

            /// <summary>
            /// 作業者のクラスをコピー
            /// </summary>
            /// <returns></returns>
            public ClassWorker Clone()
            {
                var ret = new ClassWorker();

                ret = (ClassWorker)MemberwiseClone();

                // 権限の種類をコピー
                ret.Authoritys = new Dictionary<ClassAuthority.EnumCategory, bool>(this.Authoritys);

                return ret;
            }
        }
    }
}
