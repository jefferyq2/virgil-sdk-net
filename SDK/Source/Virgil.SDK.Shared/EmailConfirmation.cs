#region Copyright (C) Virgil Security Inc.
// Copyright (C) 2015-2017 Virgil Security Inc.
// 
// Lead Maintainer: Virgil Security Inc. <support@virgilsecurity.com>
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions 
// are met:
// 
//   (1) Redistributions of source code must retain the above copyright
//   notice, this list of conditions and the following disclaimer.
//   
//   (2) Redistributions in binary form must reproduce the above copyright
//   notice, this list of conditions and the following disclaimer in
//   the documentation and/or other materials provided with the
//   distribution.
//   
//   (3) Neither the name of the copyright holder nor the names of its
//   contributors may be used to endorse or promote products derived 
//   from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE AUTHOR ''AS IS'' AND ANY EXPRESS OR
// IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT,
// INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING
// IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE.
#endregion

namespace Virgil.SDK
{
    using System.Threading.Tasks;

    using Virgil.SDK.Client;

    /// <summary>
    /// The <see cref="EmailConfirmation"/> class provides a logic to confirm the email identity.
    /// </summary>
    public class EmailConfirmation : IdentityConfirmation
    {
        private readonly string confirmationCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailConfirmation"/> class.
        /// </summary>
        public EmailConfirmation(string confirmationCode)
        {   
            this.confirmationCode = confirmationCode;
        }

        internal override async Task<string> ConfirmAndGrabValidationTokenAsync(IdentityVerificationAttempt attempt, IdentityClient client)
        {
            var tokenOption = new IdentityTokenOptions()
            {
                TimeToLive = attempt.TimeToLive,
                CountToLive = attempt.CountToLive
            };
            var tokenResult = await client.ConfirmEmailAsync(attempt.ActionId, this.confirmationCode, 
                tokenOption).ConfigureAwait(false);

            return tokenResult.ValidationToken;
        }
    }
}