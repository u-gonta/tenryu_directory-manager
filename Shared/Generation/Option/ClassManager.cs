using System.Collections.Generic;

namespace Shared
{
    namespace Generation
    {
        namespace Option
        {
            /// <summary>
            /// 生成するオプション情報を管理するクラス
            /// </summary>
            class ClassManager
            {
                /// <summary>
                /// オプションのカテゴリ番号を抽出
                /// </summary>
                /// <param name="names">オプション名のリスト</param>
                /// <returns>オプションのカテゴリ番号</returns>
                static public List<int> ExtractCategoryNumber(List<string> names)
                {
                    var ret = new List<int>();

                    // オプション名を走査
                    foreach (var name in names)
                    {
                        // オプション名を確認
                        if (name == Select.ClassDefine.UnselectedName)
                        {
                            // オプション名が未選択 ⇒ 次のオプション名へ
                            continue;
                        }

                        // オプション名のカテゴリ番号を抽出
                        var number = Tool.ClassOption.Number(name);

                        // 抽出したオプションのカテゴリ番号を確認
                        if (0 <= ret.FindIndex(target => number == target))
                        {
                            // カテゴリ番号が登録済み ⇒ 次のオプション名へ
                            continue;
                        }

                        // カテゴリ番号を追加
                        ret.Add(number);
                    }

                    return ret;
                }

                /// <summary>
                /// オプションのバリエーション番号を抽出
                /// </summary>
                /// <param name="names">オプション名のリスト</param>
                /// <param name="category">カテゴリ番号</param>
                /// <returns>オプションのバリエーション番号</returns>
                static public List<int> ExtractVariationNumber(List<string> names, int category)
                {
                    var ret = new List<int>();

                    // オプション名を走査
                    foreach (var name in names)
                    {
                        // オプション名を確認
                        if (name == Select.ClassDefine.UnselectedName)
                        {
                            // オプション名が未選択 ⇒ 次のオプション名へ
                            continue;
                        }

                        // オプションのカテゴリ番号を確認
                        if (category != Tool.ClassOption.Number(name))
                        {
                            // カテゴリ番号が不一致 ⇒ 次のオプション名へ
                            continue;
                        }

                        // オプション名のバリエーション番号を抽出
                        var number = Tool.ClassOption.Variation(name);

                        // 抽出したオプションのバリエーション番号を確認
                        if (0 <= ret.FindIndex(target => number == target))
                        {
                            // バリエーション番号が登録済み ⇒ 次のオプション名へ
                            continue;
                        }

                        // バリエーション番号を追加
                        ret.Add(number);
                    }

                    return ret;
                }
            }
        }
    }
}
