namespace Shared
{
    namespace ComboboxItem
    {
        /// <summary>
        /// コンボボックスのテンプレート用クラス
        /// </summary>
        public class ClassTemplate
        {
            /// <summary>
            /// 名称
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ClassTemplate()
            {
                this.Title = Select.ClassDefine.UnselectedName;
            }

            /// <summary>
            /// 自クラスをコピー
            /// </summary>
            /// <returns></returns>
            public virtual object Clone()
            {
                var ret = new ClassTemplate();

                ret = (ClassTemplate)MemberwiseClone();

                return ret;
            }
        }
    }
}