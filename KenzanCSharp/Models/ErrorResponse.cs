using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KenzanCSharp.Controllers
{
    public enum ErrorNumber
    {
        NONE,
        INVALID_USERNAME_OR_PASSWORD,
        NO_AUTHORIZATION_TOKEN,

        INVALID_AUTHORIZATION_TOKEN_PARSE_ERROR,
        INVALID_AUTHORIZATION_TOKEN_NO_BEARER,
        INVALID_AUTHORIZATION_TOKEN_INVALID_SIGNATURE,

        INVALID_AUTHORIZATION_HEADER_INVALID_ALGORITHM,

        INVALID_AUTHORIZATION_PAYLOAD_NO_ISSUER,
        INVALID_AUTHORIZATION_PAYLOAD_INVALID_ISSUER,
        INVALID_AUTHORIZATION_PAYLOAD_NO_ISSUED,
        INVALID_AUTHORIZATION_PAYLOAD_INVALID_ISSUED,
        INVALID_AUTHORIZATION_PAYLOAD_NO_EXPIRATION,
        INVALID_AUTHORIZATION_PAYLOAD_INVALID_EXPIRATION,
        INVALID_AUTHORIZATION_PAYLOAD_NO_USERNAME,

        INVALID_AUTHORIZATION_TOKEN_EXPIRED,

        NOT_AUTHORIZED_FOR_OPERATION,
        DUPLICATE_RECORD,
        CANNOT_DELETE_NONEXISTENT_RECORD,
        CANNOT_UPDATE_NONEXISTENT_RECORD,
        UNKNOWN_ERROR,
        CANNOT_INSERT_MISSING_FIELDS,
        CANNOT_INSERT_UNKNOWN_FIELDS
    }

    public class ErrorResponse
    {
        public ErrorResponse() { error = null; id = 0; errorcode = ErrorNumber.NONE; }
        public ErrorResponse(int id) { error = null; this.id = id; errorcode = ErrorNumber.NONE; }
        public ErrorResponse(int id, ErrorNumber errorcode, String error) { this.id = id; this.error = error; this.errorcode = errorcode; }
        public ErrorResponse(ErrorNumber errorcode, String error) { this.id = 0; this.error = error; this.errorcode = errorcode; }
        public String error { get; }
        public int id { get; }
        public ErrorNumber errorcode { get; }
    }
}