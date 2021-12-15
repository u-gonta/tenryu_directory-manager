namespace DirectoryManagerTspOption
{
    namespace ComboboxItem
    {
        /// <summary>
        /// オプション名のコンボボックス用クラス
        /// ※コンボボックスのテンプレート用クラスから継承
        /// </summary>
        public class ClassOption : Shared.ComboboxItem.ClassTemplate
        {
            /// <summary>
            /// データ
            /// </summary>
            public string Value { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ClassOption() : base()
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
                var ret = (ClassOption)base.Clone();

                ret = (ClassOption)MemberwiseClone();

                return ret;
            }
        }
    }
}
