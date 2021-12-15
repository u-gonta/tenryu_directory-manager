namespace DirectoryManagerTsp
{
    namespace ComboboxItem
    {
        namespace Equipment
        {
            /// <summary>
            /// 装置の番号のコンボボックス用クラス
            /// ※コンボボックスのテンプレート用クラスから継承
            /// </summary>
            public class ClassNumber : Shared.ComboboxItem.ClassTemplate
            {
                /// <summary>
                /// 値
                /// </summary>
                public int Value { get; set; }

                /// <summary>
                /// コンストラクタ
                /// </summary>
                public ClassNumber()
                {
                    this.Value = -1;
                }

                /// <summary>
                /// コンボボックスの項目クラスをコピー
                /// </summary>
                /// <returns></returns>
                public override object Clone()
                {
                    // 既定のクラスをコピー
                    var ret = (ClassNumber)base.Clone();

                    ret = (ClassNumber)MemberwiseClone();

                    return ret;
                }
            }
        }
    }
}
