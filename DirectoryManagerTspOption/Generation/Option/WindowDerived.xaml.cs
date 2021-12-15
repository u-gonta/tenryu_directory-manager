using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace DirectoryManagerTspOption
{
    namespace Generation
    {
        namespace Option
        {
            /// <summary>
            /// WindowDerived.xaml の相互作用ロジック
            /// </summary>
            public partial class WindowDerived : Window
            {
                /// <summary>
                /// オプションのカテゴリ番号コンボボックスの項目クラス
                /// </summary>
                private ObservableCollection<ComboboxItem.Option.ClassCategory> _categoryComboboxItems = new ObservableCollection<ComboboxItem.Option.ClassCategory>();

                /// <summary>
                /// オプションのバリエーション番号コンボボックスの項目クラス
                /// </summary>
                private ObservableCollection<ComboboxItem.Option.ClassVariation> _variationComboboxItems = new ObservableCollection<ComboboxItem.Option.ClassVariation>();

                /// <summary>
                /// オプション名
                /// </summary>
                private string _optionSource = Shared.Select.ClassDefine.UnselectedName;

                /// <summary>
                /// 選択中のカテゴリ番号
                /// </summary>
                private int _categorySelected = -1;

                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="option">オプション名</param>
                public WindowDerived(string option)
                {
                    InitializeComponent();

                    try
                    {
                        // オプション名を更新
                        _optionSource = option;

                        // カテゴリ番号を更新
                        _categorySelected = Shared.Tool.ClassOption.Number(_optionSource);

                        // 画面を初期化
                        InitializeWindow();

                        // オプションのカテゴリ番号コンボボックスを更新
                        UpdateCategoryCombobox();
                    }
                    catch (Exception ex)
                    {
                        // エラーのメッセージを表示
                        Shared.Tool.ClassException.Message(ex);
                    }
                }

                /// <summary>
                /// オプションのカテゴリ番号コンボボックスを選択
                /// </summary>
                /// <param name="sender"></param>
                /// <param name="e"></param>
                private void comboboxCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
                {
                    try
                    {
                        do
                        {
                            int selected = -1;

                            // 選択しているコンボボックスの項目を取得
                            var comboboxItem = ((ComboBox)sender).SelectedItem as ComboboxItem.Option.ClassCategory;

                            // コンボボックスの項目を確認
                            if (comboboxItem == null)
                            {
                                // コンボボックスの項目が未選択 ⇒ 処理を抜ける
                                break;
                            }

                            // コンボボックスを選択中の項目
                            selected = comboboxItem.Value;

                            // 選択中のカテゴリ番号を確認
                            if (_categorySelected != selected)
                            {
                                // カテゴリ番号が変更 ⇒ 選択中のカテゴリ番号を更新
                                _categorySelected = selected;

                                var integrationManager = Setting.ClassIntegrationManager.Instance;
                                var integration = integrationManager.GetIntegration();
                                var categoryName = "不明";

                                // カテゴリ番号を確認
                                if (integration.Categories.ContainsKey(_categorySelected))
                                {
                                    // カテゴリ番号の登録あり ⇒ カテゴリ名を取得
                                    categoryName = integration.Categories[_categorySelected];
                                }

                                // カテゴリ名を更新
                                labelCategory.Content = categoryName;

                                // オプションのバリエーション番号コンボボックスを更新
                                UpdateVariationCombobox();
                            }
                        } while (false);
                    }
                    catch (Exception ex)
                    {
                        // エラーのログを出力
                        Shared.Tool.ClassException.Logging(ex);
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
                        // 選択しているコンボボックスの項目を取得
                        var comboboxItemCategory = comboboxCategory.SelectedItem as ComboboxItem.Option.ClassCategory;

                        // コンボボックスの項目を確認
                        if (comboboxItemCategory == null)
                        {
                            // コンボボックスの項目が未選択
                            throw new Exception("カテゴリ番号が選択されていません。");
                        }

                        // 選択しているコンボボックスの項目を取得
                        var comboboxItemVariation = comboboxVariation.SelectedItem as ComboboxItem.Option.ClassVariation;

                        // コンボボックスの項目を確認
                        if (comboboxItemVariation == null)
                        {
                            // コンボボックスの項目が未選択
                            throw new Exception("バリエーション番号が選択されていません。");
                        }

                        // オプション名を確定
                        var name = labelHeader.Content + comboboxItemCategory.Title + comboboxItemVariation.Title + labelRevision.Content;

                        //@@@ オプション名の規則を確認する

                        var settingManager = Setting.ClassIntegrationManager.Instance;
                        var integration = settingManager.GetIntegration();
                        var path = "";

                        // テンプレートのディレクトリ
                        path = integration.TemplateDirectory;

                        // ディレクトリの区切りを付加
                        path = Shared.Tool.ClassPath.DirectoryDelimiter(path);

                        // オプション名を付加
                        path += name;

                        // オプション名のディレクトリを確認
                        if (Directory.Exists(path))
                        {
                            // オプション名のディレクトリが存在する ⇒ 例外を発砲
                            throw new Exception(name + "は登録済です。");
                        }

                        // 部署を走査
                        foreach (var department in integration.Departments)
                        {
                            // 部署のディレクトリ内にオプション名のディレクトリを作成
                            Directory.CreateDirectory(department.RootDirectory + Path.DirectorySeparatorChar + name);
                        }

                        // ディレクトリを作成
                        Directory.CreateDirectory(path);

                        // オプションの情報クラス
                        var information = new Shared.Generation.Option.ClassDirectory
                        {
                            // コメントを付加
                            Comment = textBoxComment.Text
                        };

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
                    // オプション名のヘッダを更新
                    labelHeader.Content = Shared.Setting.ClassOption.Header;

                    // オプション名を更新
                    labelOptionSource.Content = _optionSource;

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
                }

                /// <summary>
                /// オプションのカテゴリ番号コンボボックスを更新
                /// </summary>
                private void UpdateCategoryCombobox()
                {
                    int selected = _categorySelected;

                    // カテゴリ番号を解除
                    _categorySelected = -1;

                    // コンボボックスの項目クラスを初期化
                    _categoryComboboxItems = new ObservableCollection<ComboboxItem.Option.ClassCategory>();

                    var settingManager = Setting.ClassIntegrationManager.Instance;
                    var integration = settingManager.GetIntegration();
                    var categories = integration.Categories;
                    int index = 0;

                    // オプションのカテゴリ番号を走査
                    for (var category = 0; category <= 99; category++)
                    {
                        // オプションのカテゴリ番号を確認
                        if (categories.ContainsKey(category) == false)
                        {
                            // カテゴリ番号が未登録 ⇒ 次のカテゴリ番号へ
                            continue;
                        }

                        var comboboxItem = new ComboboxItem.Option.ClassCategory();

                        comboboxItem.Value = category;
                        comboboxItem.Title = category.ToString("D2");

                        // オプションのカテゴリ番号を確認
                        if (selected < 0)
                        {
                            // 選択中のオプションのカテゴリ番号が未初期化 ⇒ 初期化
                            selected = category;
                        }
                        if (selected == category)
                        {
                            // 選択中のオプションのカテゴリ番号と一致
                            index = _categoryComboboxItems.Count;
                        }

                        // コンボボックスの項目を追加
                        _categoryComboboxItems.Add(comboboxItem);
                    }

                    // コンボボックスの項目を更新
                    comboboxCategory.ItemsSource = _categoryComboboxItems;

                    // コンボボックスを選択
                    comboboxCategory.SelectedIndex = index;
                }

                /// <summary>
                /// オプションのバリエーション番号コンボボックスを更新
                /// </summary>
                private void UpdateVariationCombobox()
                {
                    int selected = -1;

                    // コンボボックスの項目クラスを初期化
                    _variationComboboxItems = new ObservableCollection<ComboboxItem.Option.ClassVariation>();

                    int index = 0;

                    do
                    {
                        // 選択しているコンボボックスの項目を取得
                        var comboboxCategoryItem = comboboxCategory.SelectedItem as ComboboxItem.Option.ClassCategory;

                        // コンボボックスの項目を確認
                        if (comboboxCategoryItem == null)
                        {
                            // コンボボックスの項目が未選択 ⇒ 処理を抜ける
                            break;
                        }

                        var statusManager = Status.ClassManager.Instance;
                        var hierarchy = statusManager.GetHierarchy();
                        var variations = Shared.Generation.Option.ClassManager.ExtractVariationNumber(hierarchy.GetOptionNames(), comboboxCategoryItem.Value);

                        // オプションのバリエーション番号を走査
                        for (var variation = 1; variation <= 99; variation++)
                        {
                            // オプションのバリエーション番号を確認
                            if (0 <= variations.FindIndex(target => target == variation))
                            {
                                // バリエーション番号が登録済み ⇒ 次のバリエーション番号へ
                                continue;
                            }

                            var comboboxItem = new ComboboxItem.Option.ClassVariation();

                            comboboxItem.Value = variation;
                            comboboxItem.Title = variation.ToString("D2");

                            // オプションのバリエーション番号を確認
                            if (selected < 0)
                            {
                                // 選択中のオプションのカテゴリ番号が未初期化 ⇒ 初期化
                                selected = variation;
                            }
                            if (selected == variation)
                            {
                                // 選択中のオプションのバリエーション番号と一致
                                index = _variationComboboxItems.Count;
                            }

                            // コンボボックスの項目を追加
                            _variationComboboxItems.Add(comboboxItem);
                        }
                    } while (false);

                    // コンボボックスの項目を更新
                    comboboxVariation.ItemsSource = _variationComboboxItems;

                    // コンボボックスを選択
                    comboboxVariation.SelectedIndex = index;
                }
            }
        }
    }
}
