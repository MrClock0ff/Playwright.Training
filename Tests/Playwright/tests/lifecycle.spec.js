import { expect, test } from '@playwright/test';
import UsersApi from '../constants/users.api';

test.describe.serial('Set of tests to test user account lifecycle', () => {
    const username = `lifecycle-${Date.now()}`;
    const email = `${username}@user.test`;
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

    test('Ensure user account does not exist before account creation', async () => {
        const response = await apiContext.get(UsersApi.endpoints.getAllUsers);

        expect(response.status()).toBe(200);
        expect(await response.json()).not.toContainEqual(expect.objectContaining({
            "username": username,
            "email": email
        }));
    });

    test('Ensure user account does exist after creation', async () => {
        const response = await apiContext.post(UsersApi.endpoints.createUser, {
            data: {
                "username": username,
                "email": email,
                "firstName": "life",
                "lastName": "cycle"
            }
        });

        expect(response.status()).toBe(201);

        const response2 = await apiContext.get(UsersApi.endpoints.getAllUsers);
        expect(response2.status()).toBe(200);
        expect(await response2.json()).toContainEqual(expect.objectContaining({
            "username": username,
            "email": email,
            "firstName": "life",
            "lastName": "cycle"
        }));
    });

    test('Ensure user first and last name are updated on account update', async () => {
        const response = await apiContext.get(UsersApi.endpoints.getAllUsers);
        const user = (await response.json()).find(u => u.username === username && u.email === email);

        expect(user).not.toBeUndefined();

        const response2 = await apiContext.patch(UsersApi.endpoints.updateUser.replace('{id}', user.id), {
            data: [
                {
                    "op": "replace",
                    "path": "/firstName",
                    "value": "Daniel"
                },
                {
                    "op": "replace",
                    "path": "/lastName",
                    "value": "Klokov"
                }
            ]
        });

        expect(response2.status()).toBe(200);
        expect(await response2.json()).toEqual(expect.objectContaining({
            "id": user.id,
            "username": username,
            "email": email,
            "firstName": "Daniel",
            "lastName": "Klokov"
        }));
    });

    test('Ensure user account is not present after deletion', async () => {
        const response = await apiContext.get(UsersApi.endpoints.getAllUsers);
        const user = (await response.json()).find(u => u.username === username && u.email === email);

        expect(user).not.toBeUndefined();

        const response2 = await apiContext.delete(UsersApi.endpoints.deleteUser.replace('{id}', user.id));
        expect(response2.status()).toBe(200);

        const response3 = await apiContext.get(UsersApi.endpoints.getUser.replace('{id}', user.id));
        expect(response3.status()).toBe(404);
    });
});