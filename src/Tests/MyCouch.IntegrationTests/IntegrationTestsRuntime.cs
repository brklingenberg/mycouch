﻿namespace MyCouch.IntegrationTests
{
    internal static class IntegrationTestsRuntime
    {
        internal static IClient Client { get; private set; }

        internal static void Init()
        {
            Client = TestClientFactory.CreateDefault();
            //Client.Databases.Put(TestConstants.TestDbName);
        }

        internal static void Close()
        {
            Client.Dispose();
            Client = null;
        }

        internal static void ClearAllDocuments()
        {
            using (var client = TestClientFactory.CreateDefault())
            {
                var query = client.Views.CreateSystemQuery("_all_docs");
                var response = client.Views.RunQuery<dynamic>(query);

                foreach (var row in response.Rows)
                    client.Documents.Delete(row.Id, row.Value.rev.ToString());
            }
        }
    }
}