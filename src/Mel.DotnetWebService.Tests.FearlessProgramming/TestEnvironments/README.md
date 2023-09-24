# Test Environments
Runtime elements that are practical to use when running automated tests (as opposed to running in production)

Meant to:
* decrease tests execution time
* remove configuration complexity
* allow testing a given behavior using fewer prerequisites

Examples:
* An `in-memory` database to use instead of a real database
* A permissive authentication mechanism instead of the real authentication mechanism
* A service that always returns the same result when called
