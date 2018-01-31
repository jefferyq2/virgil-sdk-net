﻿using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Virgil.CryptoAPI;
using Virgil.SDK.Common;
using Virgil.SDK.Web;
using System.Linq;

namespace Virgil.SDK.Signer
{
    public class ModelSigner
    {
        public readonly ICardCrypto Crypto;
        public const string SelfSignerType = "self";
        public const string AppSignerType = "app";
        public const string VirgilSignerType = "virgil";


        public ModelSigner(ICardCrypto crypto)
        {
            this.Crypto = crypto;
        }

        /// <summary>
        /// Signs the <see cref="RawSignedModel"/> using specified signer parameters included private key.
        /// </summary>
        public void SelfSign(RawSignedModel model, IPrivateKey signerPrivateKey, byte[] signatureSnapshot = null)
        {
            ValidateSignParams(model, signerPrivateKey);

            Sign(model,
                new SignParams()
                {
                    SignerId = CardUtils.GenerateCardId(Crypto, model.ContentSnapshot, signatureSnapshot),
                    SignerPrivateKey = signerPrivateKey,
                    SignerType = SelfSignerType
                },
                signatureSnapshot
                );
        }

        /// <summary>
        /// Signs the <see cref="RawSignedModel"/> using specified signer parameters included private key.
        /// </summary>
        public void SelfSign(RawSignedModel model, IPrivateKey signerPrivateKey, Dictionary<string, string> extraFields)
        {
            SelfSign(model, signerPrivateKey,
                (extraFields != null) ? SnapshotUtils.TakeSnapshot(extraFields) : null
                );
        }

        private static void ValidateSignParams(RawSignedModel model, IPrivateKey signerPrivateKey)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }


            if (signerPrivateKey == null)
            {
                throw new ArgumentException($"{nameof(signerPrivateKey)} property is mandatory");
            }
        }

        /// <summary>
        /// Signs the <see cref="RawSignedModel"/> using specified signer parameters.
        /// </summary>
        public void Sign(RawSignedModel model, SignParams @params, byte[] signatureSnapshot = null)
        {
            ValidateExtendedSignParams(model, @params);
            ThrowExceptionIfSignatureExists(@params, model.Signatures);

            var extendedSnapshot = signatureSnapshot != null ?
                Bytes.Combine(model.ContentSnapshot, signatureSnapshot)
                : model.ContentSnapshot;

            var signatureBytes = Crypto.GenerateSignature(extendedSnapshot, @params.SignerPrivateKey);

            var signature = new RawSignature
            {
                SignerId = @params.SignerId,
                SignerType = @params.SignerType,
                Signature = signatureBytes,
                Snapshot = signatureSnapshot
            };
            if (model.Signatures == null)
            {
                model.Signatures = new List<RawSignature>();
            }
            model.Signatures.Add(signature);
        }

        private static void ThrowExceptionIfSignatureExists(SignParams @params, IList<RawSignature> signatures)
        {
            if (signatures != null &&
                ((List<RawSignature>)signatures).Exists(
                    s => s.SignerType == @params.SignerType && s.SignerId == @params.SignerId))
            {
                throw new VirgilException("The model already has this signature.");
            }
        }

        /// <summary>
        /// Signs the <see cref="RawSignedModel"/> using specified signer parameters.
        /// </summary>
        public void Sign(RawSignedModel model, SignParams @params, Dictionary<string, string> ExtraFields)
        {
            Sign(model, @params,
                (ExtraFields != null) ? SnapshotUtils.TakeSnapshot(ExtraFields) : null
            );
        }

        private static void ValidateExtendedSignParams(RawSignedModel model, SignParams @params)
        {
            ValidateSignParams(model, @params.SignerPrivateKey);

            if (@params.SignerId == null)
            {
                throw new ArgumentException($"{nameof(@params.SignerId)} property is mandatory");
            }

            if (@params.SignerType == null)
            {
                throw new ArgumentException($"{nameof(@params.SignerType)} property is mandatory");
            }
        }
    }
}
