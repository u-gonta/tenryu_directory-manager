using System.Collections.Generic;

namespace DirectoryManagerTspOption
{
    namespace Status
    {
        /// <summary>
        /// オプションの階層を保持するクラス
        /// </summary>
        public class ClassHierarchy
        {
            /// <summary>
            /// オプション名
            /// </summary>
            private List<string> _names;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ClassHierarchy()
            {
                // 初期化
                Initialize();
            }

            /// <summary>
            /// 初期化
            /// </summary>
            public void Initialize()
            {
                // オプション名を確認
                if (this._names == null)
                {
                    this._names = new List<string>();
                }
            }

            /// <summary>
            /// オプションの階層を保持するクラスをコピー
            /// </summary>
            /// <returns></returns>
            public ClassHierarchy Clone()
            {
                var ret = new ClassHierarchy();

                ret = (ClassHierarchy)MemberwiseClone();

                // オプション名をコピー
                ret._names = new List<string>(this._names);

                return ret;
            }

            /// <summary>
            /// オプション名をクリア
            /// </summary>
            public void Clear()
            {
                // オプション名をクリア
                this._names.Clear();
            }

            /// <summary>
            /// オプション名を追加
            /// </summary>
            /// <param name="name">オプション名</param>
            public void Addition(string name)
            {
                // オプション名を追加
                this._names.Add(name);

                // 昇順で並び替え
                this._names.Sort((a, b) => a.CompareTo(b));
            }

            /// <summary>
            /// オプション名を取得
            /// </summary>
            /// <returns>オプション名の一覧</returns>
            public List<string> GetOptionNames()
            {
                var ret = new List<string>();

                // オプション名をコピー
                ret = new List<string>(_names);

                return ret;
            }
        }
    }
}
