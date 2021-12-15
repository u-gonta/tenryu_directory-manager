using System;
using System.IO;
using System.Windows;

namespace DirectoryManagerTspOption
{
    namespace Generation
    {
        namespace Option
        {
            /// <summary>
            /// WindowComment.xaml の相互作用ロジック
            /// </summary>
            public partial class WindowComment : Window
            {
                /// <summary>
                /// ディレクトリのパス
                /// </summary>
                private string _path = "";

                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="path">ディレクトリのパス</param>
                public WindowComment(string path)
                {
                    InitializeComponent();

                    try
                    {
                        // ディレクトリのパスを更新
                        _path = path;

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
                /// 更新ボタンクリック
                /// </summary>
                /// <param name="sender"></param>
                /// <param name="e"></param>
                private void buttonUpdate_Click(object sender, RoutedEventArgs e)
                {
                    try
                    {
                        // 派生するオプションのディレクトリにある情報のファイル読み込み
                        var information = Shared.Generation.Manager.ClassTemplate.LoadDirectory<Shared.Generation.Directory.ClassTemplate>(_path);

                        // コメントを付加
                        information.Comment = textBoxComment.Text;

                        // 情報のファイルを書き込み
                        Shared.Generation.Manager.ClassTemplate.SaveDirectory(information, _path);

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
                    var name = Shared.Select.ClassDefine.UnselectedName;

                    // ディレクトリのパスを確認
                    if (System.IO.Directory.Exists(_path))
                    {
                        // パスあり ⇒ オプション名を抽出
                        name = Path.GetFileNameWithoutExtension(_path);
                    }

                    // オプション名を更新
                    labelOption.Content = name;

                    // 参照するオプションのディレクトリにある情報のファイル読み込み
                    var informationSource = Shared.Generation.Manager.ClassTemplate.LoadDirectory<Shared.Generation.Option.ClassDirectory>(_path);

                    // コメントを更新
                    textBoxComment.Text = informationSource.Comment;

                    var integrationManager = Setting.ClassIntegrationManager.Instance;
                    var integration = integrationManager.GetIntegration();
                    var categoryName = "不明";

                    // カテゴリ番号を取得
                    var categoryNumber = Shared.Tool.ClassOption.Number(name);

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
}
