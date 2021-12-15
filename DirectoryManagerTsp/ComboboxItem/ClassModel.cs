namespace DirectoryManagerTsp
{
    namespace ComboboxItem
    {
        /// <summary>
        /// 機種名のコンボボックス用クラス
        /// ※コンボボックスのテンプレート用クラスから継承
        /// </summary>
        public class ClassModel : Shared.ComboboxItem.ClassTemplate
        {
            /// <summary>
            /// データ
            /// </summary>
            public string Value { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ClassModel() : base()
            {
                Value = "";
            }

            /// <summary>
            /// 自クラスをコピー
            /// </summary>
            /// <returns></returns>
            public override object Clone()
            {
                // 既定のクラスをコピー
                var ret = (ClassModel)base.Clone();

                ret = (ClassModel)MemberwiseClone();

                return ret;
            }
        }
    }
}
