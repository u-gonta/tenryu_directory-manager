using System;
using System.Diagnostics;
using System.Windows;

namespace Shared
{
    namespace Tool
    {
        /// <summary>
        /// 例外の処理をするクラス
        /// </summary>
        class ClassException
        {
            /// <summary>
            /// 例外のメッセージをログで出力
            /// </summary>
            /// <param name="ex">例外クラス</param>
            static public void Logging(Exception ex)
            {
                // ログを出力
                Debug.WriteLine(ex.Message);
            }

            /// <summary>
            /// 例外のメッセージを表示
            /// </summary>
            /// <param name="ex">例外クラス</param>
            static public void Message(Exception ex)
            {
                // 例外のメッセージをログで出力
                Logging(ex);

                // メッセージを表示
                MessageBox.Show(ex.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
