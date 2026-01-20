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

    test('Create user should return bad request response for a malformed username', async ({request}) => {
        const response = await apiContext.post(UsersApi.endpoints.createUser, {
            data: {
                username: 'test with space',
                firstName: 'test',
                lastName: 'test',
                email: 'test@test.test',
            }
        });

        expect(response.status()).toBe(400);
        expect((await response.json()).title).toBe('Validation failed: \n -- Username: \'Username\' is not in the correct format. Severity: Error');
        expect((await response.json()).detail).toBe('Validation failed: \n -- Username: \'Username\' is not in the correct format. Severity: Error');
    });

    test('Create user should return bad request response for a malformed email', async ({request}) => {
        const response = await apiContext.post(UsersApi.endpoints.createUser, {
            data: {
                username: 'test',
                firstName: 'test',
                lastName: 'test',
                email: 'test-test.test',
            }
        });

        expect(response.status()).toBe(400);
        expect((await response.json()).title).toBe('Validation failed: \n -- Email: \'Email\' is not a valid email address. Severity: Error');
        expect((await response.json()).detail).toBe('Validation failed: \n -- Email: \'Email\' is not a valid email address. Severity: Error');
    });

    test('Create user should return bad response for malformed first name', async ({request}) => {
        const response = await apiContext.post(UsersApi.endpoints.createUser, {
            data: {
                username: 'test',
                firstName: 'test-123',
                lastName: 'test',
                email: 'test@test.test'
            }
        });

        expect(response.status()).toBe(400);
        expect((await response.json()).title).toBe('Validation failed: \n -- FirstName: \'First Name\' is not in the correct format. Severity: Error');
        expect((await response.json()).detail).toBe('Validation failed: \n -- FirstName: \'First Name\' is not in the correct format. Severity: Error');
    });

    test('Create user should return bad response for malformed last name', async ({request}) => {
        const response = await apiContext.post(UsersApi.endpoints.createUser, {
            data: {
                username: 'test',
                firstName: 'test',
                lastName: 'test-123',
                email: 'test@test.test'
            }
        });

        expect(response.status()).toBe(400);
        expect((await response.json()).title).toBe('Validation failed: \n -- LastName: \'Last Name\' is not in the correct format. Severity: Error');
        expect((await response.json()).detail).toBe('Validation failed: \n -- LastName: \'Last Name\' is not in the correct format. Severity: Error');
    });

    test('Create user should return bad response for missing required values', async ({request}) => {
        const response = await apiContext.post(UsersApi.endpoints.createUser, {
            data: {
            }
        });

        expect(response.status()).toBe(400);
        expect((await response.json()).errors).toEqual(expect.objectContaining({
            Username: [
                'The Username field is required.'
            ],
            Email: [
                'The Email field is required.'
            ],
            LastName: [
                'The LastName field is required.'
            ],
            FirstName: [
                'The FirstName field is required.'
            ]
        }));
    });

    test('Ensure default user exist', async () => {
        const response = await apiContext.get(UsersApi.endpoints.getUser.replace('{id}', '5c33e35c-606e-4d8f-91ec-1eba45094043'));

        expect(response.status()).toBe(200);
        expect(await response.json()).toEqual(expect.objectContaining({
            "email": "daniel@training.com",
            "id": "5c33e35c-606e-4d8f-91ec-1eba45094043",
            "username": "daniel-training",
        }))
    });
});