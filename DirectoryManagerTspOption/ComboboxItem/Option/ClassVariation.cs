namespace DirectoryManagerTspOption
{
    namespace ComboboxItem
    {
        namespace Option
        {
            /// <summary>
            /// オプションのバリエーション番号コンボボックス用クラス
            /// ※コンボボックスのテンプレート用クラスから継承
            /// </summary>
            public class ClassVariation : Shared.ComboboxItem.ClassTemplate
            {
                /// <summary>
                /// データ
                /// </summary>
                public int Value { get; set; }

                /// <summary>
                /// コンストラクタ
                /// </summary>
                public ClassVariation() : base()
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
                    var ret = (ClassVariation)base.Clone();

                    ret = (ClassVariation)MemberwiseClone();

                    return ret;
                }
            }
        }
    }
}
