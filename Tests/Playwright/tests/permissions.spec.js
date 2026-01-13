import {expect, test} from "@playwright/test";
import UsersApi from "../constants/users.api";

test.describe('Endpoint permissions tests', () => {
    let apiContext;

    // Setup
    test.beforeAll(async ({ playwright}) => {
        apiContext = await playwright.request.newContext({
            baseURL: UsersApi.baseUrl,
            extraHTTPHeaders: {
                Accept: 'application/json',
                // Use access token which doesn't have required permissions
                Authorization: `Bearer ${process.env.USER2_ACCESS_TOKEN}`,
            }
        });
    });

    // Teardown
    test.afterAll(async () => {
        await apiContext.dispose();
    });

    test('Get all users should return forbidden response for a request with insufficient permissions', async ({request}) => {
        const response = await apiContext.get(UsersApi.endpoints.getAllUsers);

        expect(response.status()).toBe(403);
    });

    test('Get user should return forbidden response for a request with insufficient permissions', async ({request}) => {
        const response = await apiContext.get(UsersApi.endpoints.getUser.replace('{id}', UsersApi.defaultUserId));

        expect(response.status()).toBe(403);
    });

    test('Create user should return forbidden response for a request with insufficient permissions', async ({request}) => {
        const response = await apiContext.post(UsersApi.endpoints.createUser, {
            data: {
                username: 'test',
                firstName: 'test',
                lastName: 'test',
                email: 'test@test.test',
            }
        });

        expect(response.status()).toBe(403);
    });

    test('Delete user should return forbidden response for a request with insufficient permissions', async ({request}) => {
        const response = await apiContext.delete(UsersApi.endpoints.deleteUser.replace('{id}', UsersApi.defaultUserId));

        expect(response.status()).toBe(403);
    });

    test('Update user should return forbidden response for a request with insufficient permissions', async ({request}) => {
        const response = await apiContext.patch(UsersApi.endpoints.updateUser.replace('{id}', UsersApi.defaultUserId), {
            data: [
                {
                    "op": "replace",
                    "path": "/firstName",
                    "value": "updatedTest"
                }
            ]
        });

        expect(response.status()).toBe(403);
    });
});