using Gumblr.DataAccess;
using Gumblr.Helpers;
using Gumblr.Models;
using Gumblr.Storage.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Gumblr.Controllers
{
    public class TestController : Controller
    {
        private ITableProvider mTableProvider;
        private IUserRepository mUserRepository;

        public TestController(IConfigurationRetriever aConfigurationRetriever, TableStorageProvider.Factory aTableProviderFactory, IUserRepository aUserRepository)
        {
            mTableProvider = aTableProviderFactory(aConfigurationRetriever, "MatchStatistics");
            mUserRepository = aUserRepository;
        }

        public List<string> MatchIds
        {
            get
            {
                return new List<string> {
                "079d8b85-4930-43d8-8c3c-e3775e17852e",
                "0966b241-812b-48d7-a0e9-08b6f230f767",
                "0ce1b1f5-5984-4dbc-b59e-a354496d01b8",
                "0e1d1467-1bee-478b-9327-834825310d32",
                "13e183fa-c2d9-4ce3-bd1c-724cbbbaf359",
                "19c8bc45-0acf-423c-b99c-076445d0dd3a",
                "290a27e8-ca0f-4fc8-ae88-0feb0ea34e85",
                "4474f2ac-6790-46fc-8221-5306ad841ae4",
                "4685f423-b8d4-48a0-8d88-36f0eed793e9",
                "47a89312-eddf-44cc-97d4-86b91951f180",
                "4de327f3-fd73-435b-afcb-e593b0eb72f1",
                "537a8f02-c6f2-44fe-bc5c-266369137632",
                "54ae44d9-e886-4841-84c1-51dd8bfaac2c",
                "563cc16c-17c3-495f-a2bc-f3012dd90352",
                "592cc41b-bf65-45ed-a5a6-1575fbd8fdb4",
                "59f53401-4877-4d93-bad4-95b5f19d7218",
                "5b082b4f-04c2-4277-a6dd-bd30804a57f3",
                "5c1ee679-518a-4417-b09b-b3bdd13bd177",
                "62c5dc0d-9f1e-4e10-a77f-cd3e745b8b1f",
                "64b6cc3c-f771-4a90-8269-348a3ba8182a",
                "6a80867e-c502-4ce9-861c-01621e02dbde",
                "6ceb36de-de2f-4225-8acb-5bddcee05a6c",
                "6cfd53d9-5be5-40a8-96e1-934cd4f44501",
                "70383364-1291-4615-8a05-6c96e34a3f38",
                "72540a61-3af7-4a4f-9e3e-a14ec75b510e",
                "73ba3605-c996-4240-8aca-81537db3dc04",
                "7605fd05-81eb-4eb9-96ee-ca75705289ac",
                "7670f4ce-4c68-4747-9862-ac2144c95168",
                "782be4b0-0b0b-4851-8b57-43fd5e998da0",
                "7b85fa4a-c743-4af4-aadb-4321f8d3bd79",
                "7b90a72e-48b0-4eb0-b263-ff0b9cc00f31",
                "7c8e6559-9027-42eb-880f-3f646e78734d",
                "913b59fa-b9fe-48e9-9841-0cef83f1a6d5",
                "94505027-0cb1-4e98-a073-97de836c7db5",
                "995b5a30-f208-4cbb-b307-332cf422104b",
                "9a7d0a1b-05fc-43a0-92cb-bcdb75bc8a9e",
                "9b1258e5-96c1-44a7-b942-0ec2954a33e8",
                "a1976f56-e43b-44d7-951f-f231961bf3cf",
                "a33880f2-b155-4f25-90a2-a1623d7ab3b1",
                "a3a9c8a6-588e-4b6a-a317-d3f18076e677",
                "addc7c78-8e0d-4533-a2d9-b3b72d9b2112",
                "b14f9386-4c8c-438f-a87a-df0eb9fd73ba",
                "b6aace8e-e5bc-4410-b05f-a96c6474d62a",
                "bc949c06-2c70-45cf-81b5-9b00ae224364",
                "be317414-6ce3-4ae2-a7c4-b2d466b1a2e9",
                "bed87b9d-1125-43bd-af66-303f452e1fe6",
                "c0bf7540-f3a0-49ed-abd2-46c57466579a",
                "c13ef5c3-cb46-45df-b10e-fd3b0cd8af32",
                "ca9c421d-da68-40bd-90fc-1b7fc9a2c09e",
                "d01e57c1-6433-441e-b0cb-54f460092be5",
                "d151f985-b024-4104-890a-9d9d381b66c7",
                "d2caed49-c095-4630-a87c-e80ce70690cd",
                "d4101772-7f15-4c95-886e-7e85b6072cca",
                "d55c4e13-f3ad-4a95-b1c8-2b0b0da9fef2",
                "dc9995f0-9740-439f-b4cb-deae1ba5ab53",
                "e7de455e-33eb-4ce5-87c8-2ba7339445ae",
                "e8fca50d-82be-4fcd-9bfa-d39ae13333f1",
                "e990990c-90bf-44d7-8479-de0f1e41cf24",
                "ea441a1e-afc4-4414-9bc5-c75db2535775",
                "ec2af4d8-6d72-4be3-bbe3-88e2885407c1",
                "f101d8f0-aac4-4fff-b761-d12f010eb11f",
                "f2ad12dc-d5a4-4d8f-a544-025577de3582",
                "f62eb22a-67fc-4f82-a11e-b97e621cbed4",
                "fa8a6eb5-bac3-4704-b5a6-9463ee14e0f3",
            };
            }
        }

        public async Task<ActionResult> UpdateStatisticsTable()
        {
            var users = await mUserRepository.GetAllUsers();
            var usersById = users.ToDictionary(x => x.Id);

            foreach (var matchId in MatchIds)
            {
                var partition = await mTableProvider.GetPartition(matchId);
                foreach (var row in partition)
                {
                    if (row.Properties.ContainsKey("Username")) continue;

                    var userId = row.Properties["UserId"] as string;
                    ApplicationUser user;
                    if (usersById.TryGetValue(userId, out user))
                    {
                        row.Properties["Username"] = user.UserName;
                        await mTableProvider.Update(matchId, userId, row);
                    }
                }
            }

            return Json("true");
        }
	}
}