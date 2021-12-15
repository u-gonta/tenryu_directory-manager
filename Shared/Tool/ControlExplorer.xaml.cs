using Microsoft.WindowsAPICodePack.Controls;
using Microsoft.WindowsAPICodePack.Controls.WindowsForms;
using Microsoft.WindowsAPICodePack.Shell;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;

namespace Shared
{
    namespace Tool
    {
        /// <summary>
        /// ControlExplorer.xaml の相互作用ロジック
        /// </summary>
        public partial class ControlExplorer : UserControl
        {
            /// <summary>
            /// エクスプローラ
            /// </summary>
            ExplorerBrowser _explorerBrowser = new ExplorerBrowser();

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ControlExplorer()
            {
                InitializeComponent();
            }

            /// <summary>
            /// コントロールのロード
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void UserControl_Loaded(object sender, RoutedEventArgs e)
            {
                var host = new WindowsFormsHost();

                // 「新しいフォルダ」などのコマンドが表示されるツールバーを非表示
                _explorerBrowser.NavigationOptions.PaneVisibility.Commands = PaneVisibilityState.Hide;
                // 「整理」メニューをCommandsツールバーを非表示 ※Commandsに依存している
                //_explorerBrowser.NavigationOptions.PaneVisibility.CommandsOrganize = PaneVisibilityState.Hide;
                // 「大アイコン」「詳細」「並べて表示」などの表示切替ボタンをCommandsツールバーを非表示 ※Commandsに依存している
                //_explorerBrowser.NavigationOptions.PaneVisibility.CommandsView = PaneVisibilityState.Hide;
                // システム情報、ファイル情報、画像プレビュー、画像サイズなどを非表示
                _explorerBrowser.NavigationOptions.PaneVisibility.Details = PaneVisibilityState.Hide;
                // 左側に表示されるツリービューを非表示
                _explorerBrowser.NavigationOptions.PaneVisibility.Navigation = PaneVisibilityState.Hide;
                // プレビュー(右側に表示)、画像プレビューを非表示
                _explorerBrowser.NavigationOptions.PaneVisibility.Preview = PaneVisibilityState.Hide;

                // Windowsフォームコントロールのホストにエクスプローラを割り当てる
                host.Child = _explorerBrowser;

                _explorerBrowser.Visible = true;

                // Windowsフォームコントロールを追加
                this.grid.Children.Add(host);
            }

            /// <summary>
            /// 表示するディレクトリを更新
            /// </summary>
            /// <param name="directory">ディレクトリのパス</param>
            public void Navigate(string directory)
            {
                // ディレクトリを確認
                if (Directory.Exists(directory) == false)
                {
                    // ディレクトリがない
                    _explorerBrowser.Visible = false;
                }
                else
                {
                    // ディレクトリがある
                    _explorerBrowser.Navigate(ShellObject.FromParsingName(directory));
                    _explorerBrowser.Visible = true;
                }
            }
        }
    }
}
