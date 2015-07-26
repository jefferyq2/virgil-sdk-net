﻿namespace Virgil.SDK.PrivateKeys.Tests
{
    using System;
    using System.Text;

    using FluentAssertions;

    using NUnit.Framework;

    using Virgil.Crypto;
    using Virgil.SDK.PrivateKeys.Http;
    using Virgil.SDK.PrivateKeys.Model;

    public class AccountTests
    {
        private const string URL = "https://keys-private-stg.virgilsecurity.com/v2/";
        private const string TestUserId = "test-virgil@divermail.com";
        private const string TestPassword = "12345678";

        private readonly Guid TestAccountId = Guid.Parse("2775e79c-ffba-877a-d183-e4fad453e266");
        private readonly Guid TestPublicKeyId = Guid.Parse("d2aa2087-83c9-7bb7-2982-036049d73ede");

        private readonly byte[] PrivateKey = Convert.FromBase64String(
            "LS0tLS1CRUdJTiBFQyBQUklWQVRFIEtFWS0tLS0tCk1JSGFBZ0VCQkVBV1hYR05Hb1NKREluMXc2YytWaWxRa2pjSnJ0cmkvZVJFZzFrdS9UaTE2c255US9sRU0zb1QKS201MDMyNGhxb2JoaDhKSDNNV2p6T1J4LzVWaGFvVEFvQXNHQ1Nza0F3TUNDQUVCRGFHQmhRT0JnZ0FFbjhveQp1RGwwRllXSWFKczdwWFNJUDRDZGF4KytWend4T1g4YTU2R2tJSUhBem12RnZ6WlZQOEJLcjVEVkhlZXRKdkRSCmIrcVR3eTZWUWZEdGRIL0wxVTdmTHROZENCSTEzRDZCL0o1VDBBRndQZnYrUWNRWFFsTytMek9KN1dGOHhkb2kKWVlXTmtwaGFCYkZseE0vbHo0TkVDM1ZwVEMzU01wSTBmR2tyNERNPQotLS0tLUVORCBFQyBQUklWQVRFIEtFWS0tLS0tCg==");
        
        [Test]
        public async void Should_CreateEasyAccount_By_GivenParameters()
        {
            var client = new KeyringClient(new Connection(new Uri(URL)));

            var signer = new VirgilSigner();
            var sign = signer.Sign(Encoding.UTF8.GetBytes(TestPublicKeyId.ToString()), PrivateKey);

            await client.Accounts.Create(TestAccountId, PrivateKeysAccountType.Easy, TestPublicKeyId, sign, TestPassword);
            
            var credentials = new Credentials("test-virgil@divermail.com", "12345678");
            client.Connection.SetCredentials(credentials);

            var createdAccount = await client.Accounts.Get(TestAccountId);

            createdAccount.Should().NotBeNull();
            createdAccount.AccountId.Should().Be(TestAccountId);
            createdAccount.Type.Should().Be(PrivateKeysAccountType.Easy);
            createdAccount.PrivateKeys.Should().BeEmpty();

            await client.Accounts.Remove(TestAccountId, TestPublicKeyId, sign);
        }

        [Test]
        public async void Should_CreateNormalAccount_By_GivenParameters()
        {
            var client = new KeyringClient(new Connection(new Uri(URL)));

            var signer = new VirgilSigner();
            var sign = signer.Sign(Encoding.UTF8.GetBytes(TestPublicKeyId.ToString()), PrivateKey);

            await client.Accounts.Create(TestAccountId, PrivateKeysAccountType.Normal, TestPublicKeyId, sign, "12345678");

            var credentials = new Credentials("test-virgil@divermail.com", "12345678");
            client.Connection.SetCredentials(credentials);

            var createdAccount = await client.Accounts.Get(TestAccountId);

            createdAccount.Should().NotBeNull();
            createdAccount.AccountId.Should().Be(TestAccountId);
            createdAccount.Type.Should().Be(PrivateKeysAccountType.Normal);
            createdAccount.PrivateKeys.Should().BeEmpty();

            await client.Accounts.Remove(TestAccountId, TestPublicKeyId, sign);
        }

        [Test]
        public async void Should_DeleteExistingAccount()
        {
            var client = new KeyringClient(new Connection(new Uri(URL)));

            var signer = new VirgilSigner();
            var sign = signer.Sign(Encoding.UTF8.GetBytes(TestPublicKeyId.ToString()), PrivateKey);

            await client.Accounts.Create(TestAccountId, PrivateKeysAccountType.Easy, TestPublicKeyId, sign, "12345678");

            var credentials = new Credentials("test-virgil@divermail.com", "12345678");
            client.Connection.SetCredentials(credentials);

            await client.Accounts.Remove(TestAccountId, TestPublicKeyId, sign);
        } 
    }
}