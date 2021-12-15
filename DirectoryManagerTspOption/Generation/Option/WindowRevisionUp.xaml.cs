using System;
using System.IO;
using System.Windows;

namespace DirectoryManagerTspOption
{
    namespace Generation
    {
        /// <summary>
        /// WindowOptionRevisionUp.xaml の相互作用ロジック
        /// </summary>
        public partial class WindowOptionRevisionUp : Window
        {
            /// <summary>
            /// オプション名
            /// </summary>
            private string _optionSource = Shared.Select.ClassDefine.UnselectedName;

            /// <summary>
            /// リビジョンアップ後のオプション名
            /// </summary>
            private string _optionDestination = Shared.Select.ClassDefine.UnselectedName;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="option">オプション名</param>
            public WindowOptionRevisionUp(string option)
            {
                InitializeComponent();

                try
                {
                    // オプション名を更新
                    _optionSource = option;

                    // リビジョンアップ後のオプション名を更新
                    _optionDestination = Shared.Tool.ClassOption.Rename(_optionSource, Shared.Tool.ClassOption.Revision(_optionSource) + 1);

                    // 画面を初期化
                    InitializeWindow();
                }
                catch (Exception ex)
                {
                    // エラーのメッセージを表示
                    Shared.Tool.ClassException.Message(ex);
                }
            }

            /// <summary>
            /// 作成ボタンクリック
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void buttonGeneration_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    var settingManager = Setting.ClassIntegrationManager.Instance;
                    var integration = settingManager.GetIntegration();

                    // 部署を走査
                    foreach (var department in integration.Departments)
                    {
                        // 部署のディレクトリ内にオプションのディレクトリを作成
                        Directory.CreateDirectory(department.RootDirectory + Path.DirectorySeparatorChar + _optionDestination);

                        // リビジョンアップ
                        ClassManager.RevisionUp(department.RootDirectory, _optionDestination);
                    }

                    var path = "";

                    // テンプレートのディレクトリ
                    path = integration.TemplateDirectory;

                    // ディレクトリの区切りを付加
                    path = Shared.Tool.ClassPath.DirectoryDelimiter(path);

                    // オプション名を付加
                    path += _optionDestination;

                    // ディレクトリを作成
                    Directory.CreateDirectory(path);

                    // 派生するオプションのディレクトリにある情報のファイル読み込み
                    var information = ClassManager.LoadDirectory(path);

                    // 派生元のオプション名を付加
                    information.Derivation = _optionSource;

                    // コメントを付加
                    information.Comment = textBoxComment.Text;

                    // 情報のファイルを書き込み
                    Shared.Generation.Manager.ClassTemplate.SaveDirectory(information, path);

                    // 戻り値を更新
                    this.DialogResult = true;

                    // 画面を閉じる
                    this.Close();
                }
                catch (Exception ex)
                {
                    // エラーのメッセージを表示
                    Shared.Tool.ClassException.Message(ex);
                }
            }

            /// <summary>
            /// 画面を初期化
            /// </summary>
            private void InitializeWindow()
            {
                // オプション名を更新
                labelOptionSource.Content = _optionSource;

                // リビジョンアップ後のオプション名を更新
                labelOptionDestination.Content = _optionDestination;

                var settingManager = Setting.ClassIntegrationManager.Instance;
                var integration = settingManager.GetIntegration();
                var path = "";

                // テンプレートのディレクトリ
                path = integration.TemplateDirectory;

                // ディレクトリの区切りを付加
                path = Shared.Tool.ClassPath.DirectoryDelimiter(path);

                // オプション名を付加
                path += _optionSource;

                // 参照する装置ディレクトリにある情報のファイル読み込み
                var informationSource = ClassManager.LoadDirectory(path);

                // コメントを更新
                textBoxComment.Text = informationSource.Comment;

                var categoryName = "不明";

                // カテゴリ番号を取得
                var categoryNumber = Shared.Tool.ClassOption.Number(_optionSource);

                // カテゴリ番号を確認
                if (integration.Categories.ContainsKey(categoryNumber))
                {
                    // カテゴリ番号の登録あり ⇒ カテゴリ名を取得
                    categoryName = integration.Categories[categoryNumber];
                }

                // カテゴリ名を更新
                labelCategory.Content = categoryName;
            }
        }
    }
}
