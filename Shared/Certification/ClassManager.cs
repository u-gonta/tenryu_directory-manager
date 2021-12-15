using Microsoft.Identity.Client;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Shared
{
    namespace Certification
    {
        /// <summary>
        /// ユーザー認証を管理するクラス
        ///  ※シングルトンのテンプレート用クラスから継承
        /// </summary>
        public class ClassManager : Singleton.ClassTemplate<ClassManager>
        {
            /// <summary>
            /// クライアント(アプリケーション)のID
            /// </summary>
            private const string ClientId = "6a205f4f-6066-4123-934a-e0e76e2fb381";

            /// <summary>
            /// テナントのID
            /// </summary>
            private const string TenantId = "4034a6b4-8d7b-4038-9d7b-80e05d585d0e";

            /// <summary>
            /// 認証を行うクライアント
            /// </summary>
            private IPublicClientApplication _app;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ClassManager()
            {
                // 初期化
                Initialize();
            }

            /// <summary>
            /// 初期化
            /// </summary>
            private void Initialize()
            {
                // 認証を行うクライアントを確認
                if (_app == null)
                {
                    // 認証を行うクライアントなし ⇒ 構築
                    _app = PublicClientApplicationBuilder.Create(ClientId)
                        .WithAuthority(AzureCloudInstance.AzurePublic, TenantId)
                        .WithDefaultRedirectUri()
                        .Build();
                }
            }

            /// <summary>
            /// ユーザーIDを取得
            /// </summary>
            /// <returns>ユーザーID</returns>
            public async Task<string> AcquireToken()
            {
                string ret = "";

                AuthenticationResult result = null;

                do
                {
                    string[] scopes = new string[] { "User.Read" };

                    // 全てのIAccountオブジェクトを取得
                    var accounts = await _app.GetAccountsAsync().ConfigureAwait(false);

                    // 先頭のIAccountオブジェクトを取得
                    var firstAccount = accounts.FirstOrDefault();

                    //@@@ ログアウトしたらキャッシュからトークンを取得しない
                    //try
                    //{
                    //    // キャッシュからトークンを取得
                    //    result = await _app.AcquireTokenSilent(scopes, firstAccount)
                    //                .ExecuteAsync().ConfigureAwait(false);

                    //    // 処理を抜ける
                    //    break;
                    //}
                    //catch (MsalUiRequiredException ex)
                    //{
                    //    // キャッシュからトークンの取得に失敗 ⇒ エラーのログを出力
                    //    Tool.ClassException.Logging(ex);
                    //}
                    //catch (Exception ex)
                    //{
                    //    // エラーのログを出力
                    //    Tool.ClassException.Logging(ex);

                    //    // 処理を抜ける
                    //    break;
                    //}

                    try
                    {
                        // サインインを実行
                        result = await _app.AcquireTokenInteractive(scopes)
                                    .WithAccount(accounts.FirstOrDefault())
                                    .WithPrompt(Prompt.SelectAccount)
                                    .ExecuteAsync().ConfigureAwait(false);

                        // 処理を抜ける
                        break;
                    }
                    catch (Exception ex)
                    {
                        // エラーのログを出力
                        Tool.ClassException.Logging(ex);

                        // 処理を抜ける
                        break;
                    }
                } while (false);

                // トークンを確認
                if (result != null)
                {
                    // トークンあり ⇒ ユーザー名を取得
                    ret = result.Account.Username;
                }

                return ret;
            }
        }
    }
}
