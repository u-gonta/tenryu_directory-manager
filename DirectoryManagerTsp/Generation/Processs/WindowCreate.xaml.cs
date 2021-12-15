using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace DirectoryManagerTsp
{
    namespace Generation
    {
        namespace Process
        {
            /// <summary>
            /// WindowCreate.xaml の相互作用ロジック
            /// </summary>
            public partial class WindowCreate : Window
            {
                /// <summary>
                /// 機種名
                /// </summary>
                private string _model = Shared.Select.ClassDefine.UnselectedName;

                /// <summary>
                /// 装置名
                /// </summary>
                private string _equipment = Shared.Select.ClassDefine.UnselectedName;

                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="model">機種名</param>
                /// <param name="equipment">装置名</param>
                public WindowCreate(string model, string equipment)
                {
                    InitializeComponent();

                    try
                    {
                        // 機種名を更新
                        _model = model;

                        // 装置名を更新
                        _equipment = equipment;

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
                        // 工番を取得
                        var process = textBoxName.Text;

                        // 工番を確認
                        if (process.Length <= 0)
                        {
                            // 工番が未入力
                            throw new Exception("工番が未入力です。");
                        }

                        // 工番の桁数を確認
                        if (process.Length < 6)
                        {
                            // 工番の桁数が少ない
                            throw new Exception("工番の桁数が足りません。");
                        }

                        var settingManager = Setting.ClassIntegrationManager.Instance;
                        var integration = settingManager.GetIntegration();

                        // 装置ディレクトリを取得
                        var path = GetEquipmentPath();

                        // ディレクトリの区切りを付加
                        path = Shared.Tool.ClassPath.DirectoryDelimiter(path);

                        // 工番のディレクトリを付加
                        path += integration.TemplateProcessDirectory;

                        // ディレクトリの区切りを付加
                        path = Shared.Tool.ClassPath.DirectoryDelimiter(path);

                        // 工番名を付加
                        path += process;

                        // 工番のディレクトリを確認
                        if (Directory.Exists(path))
                        {
                            // 工番のディレクトリがある
                            throw new Exception(process + "は登録済です。");
                        }

                        // 部署を走査
                        foreach (var department in integration.Departments)
                        {
                            // 装置のディレクトリ内に工番のディレクトリを作成
                            Directory.CreateDirectory(Shared.Tool.ClassPath.DirectoryDelimiter(department.RootDirectory) + Shared.Tool.ClassPath.DirectoryDelimiter(_model) + Shared.Tool.ClassPath.DirectoryDelimiter(_equipment) + Shared.Tool.ClassPath.DirectoryDelimiter(department.ProcessDirectory) + process);
                        }

                        // ディレクトリを作成
                        Directory.CreateDirectory(path);

                        // 派生後の装置ディレクトリにある情報のファイル読み込み
                        var information = ClassManager.LoadDirectory(path);

                        // コメントを付加
                        information.Comment = textBoxComment.Text;

                        // 情報のファイルを書き込み
                        ClassManager.SaveDirectory(information, path);

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
                    // 機種名を更新
                    labelModel.Content = _model;

                    // 装置名を更新
                    labelEquipment.Content = _equipment;

                    // 機種ディレクトリを取得
                    var path = GetModelPath();

                    // 機種ディレクトリにある情報のファイル読み込み
                    var information = ClassManager.LoadDirectory(path);

                    // 機種のコメントを更新
                    labelModelComment.Content = information.Comment;

                    // 装置ディレクトリを取得
                    path = GetEquipmentPath();

                    // 装置ディレクトリにある情報のファイル読み込み
                    information = ClassManager.LoadDirectory(path);

                    // 派生元の装置名を確認
                    if (0 < information.DerivationEquipment.Length)
                    {
                        // 派生元の装置名がある ⇒ 派生元の装置名を付加
                        labelEquipmentComment.Content = "派生元：" + information.DerivationEquipment;
                        labelEquipmentComment.Content += "\n";
                    }

                    // コメントを更新
                    labelEquipmentComment.Content += information.Comment;

                    // 工番名へフォーカスを移動
                    textBoxName.Focus();
                }

                /// <summary>
                /// 機種ディレクトリを取得
                /// </summary>
                /// <returns>機種ディレクトリ</returns>
                private string GetModelPath()
                {
                    string ret = "";

                    var settingManager = Setting.ClassIntegrationManager.Instance;
                    var integration = settingManager.GetIntegration();

                    // テンプレートのルートディレクトリ
                    ret = integration.TemplateRootDirectory;

                    // ディレクトリの区切りを付加
                    ret = Shared.Tool.ClassPath.DirectoryDelimiter(ret);

                    // 機種名を付加
                    ret += _model;

                    return ret;
                }

                /// <summary>
                /// 装置ディレクトリを取得
                /// </summary>
                /// <returns>装置ディレクトリ</returns>
                private string GetEquipmentPath()
                {
                    string ret = "";

                    // 機種ディレクトリを取得
                    ret = GetModelPath();

                    // ディレクトリの区切りを付加
                    ret = Shared.Tool.ClassPath.DirectoryDelimiter(ret);

                    // 装置名を付加
                    ret += _equipment;

                    return ret;
                }
            }
        }
    }
}
