import {expect, test} from "@playwright/test";
import UsersApi from "../constants/users.api";

test.describe('Endpoint validation tests.', () => {
    let apiContext;
    
    // Setup
    test.beforeAll(async ({ playwright}) => {
        apiContext = await playwright.request.newContext({
            baseURL: UsersApi.baseUrl,
            extraHTTPHeaders: {
                Accept: 'application/json',
                // Use access token which does have required permissions
                Authorization: `Bearer ${process.env.USER1_ACCESS_TOKEN}`,
            }
        });
    });

    // Teardown
    test.afterAll(async () => {
        await apiContext.dispose();
    });

    test('Get user should return bad request response for a malformed user ID.', async ({request}) => {
        const response = await apiContext.get(UsersApi.endpoints.getUser.replace('{id}', 'malformed-user-id'));

        expect(response.status()).toBe(400);
    });
    
    test('Get user should return not found response for a non existing user.', async ({request}) => {
        const response = await apiContext.get(UsersApi.endpoints.getUser.replace('{id}', '3e077171-641f-4187-86f6-53cbca80e07b'));

        expect(response.status()).toBe(404);
    });
});