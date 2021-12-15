namespace DirectoryManagerTspOption
{
    namespace ComboboxItem
    {
        namespace Option
        {
            /// <summary>
            /// オプションのカテゴリ番号コンボボックス用クラス
            /// ※コンボボックスのテンプレート用クラスから継承
            /// </summary>
            public class ClassCategory : Shared.ComboboxItem.ClassTemplate
            {
                /// <summary>
                /// データ
                /// </summary>
                public int Value { get; set; }

                /// <summary>
                /// コンストラクタ
                /// </summary>
                public ClassCategory() : base()
                {
                    this.Value = -1;
                }

                /// <summary>
                /// 自クラスをコピー
                /// </summary>
                /// <returns></returns>
                public override object Clone()
                {
                    // 既定のクラスをコピー
                    var ret = (ClassCategory)base.Clone();

                    ret = (ClassCategory)MemberwiseClone();

                    return ret;
                }
            }
        }
    }
}
