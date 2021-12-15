namespace DirectoryManagerTsp
{
    namespace ComboboxItem
    {
        /// <summary>
        /// 装置名のコンボボックス用クラス
        /// ※コンボボックスのテンプレート用クラスから継承
        /// </summary>
        public class ClassEquipment : Shared.ComboboxItem.ClassTemplate
        {
            /// <summary>
            /// 機種名
            /// </summary>
            public string Model { get; set; }

            /// <summary>
            /// 装置名
            /// </summary>
            public string Value { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ClassEquipment()
            {
                this.Model = Shared.Select.ClassDefine.UnselectedName;
                this.Value = Shared.Select.ClassDefine.UnselectedName;
            }

            /// <summary>
            /// コンボボックスの項目クラスをコピー
            /// </summary>
            /// <returns></returns>
            public override object Clone()
            {
                // 既定のクラスをコピー
                var ret = (ClassEquipment)base.Clone();

                ret = (ClassEquipment)MemberwiseClone();

                return ret;
            }
        }
    }
}
