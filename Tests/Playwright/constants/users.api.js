export default class UsersApi {
    static baseUrl= 'http://localhost:8081';
    
    static endpoints = {
      getAllUsers: '/api/v1/users',  
      getUser: '/api/v1/users/{id}',
      createUser: '/api/v1/users',
      deleteUser: '/api/v1/users/{id}',
      updateUser: '/api/v1/users/{id}'  
    };
    
    static defaultUserId = '5c33e35c-606e-4d8f-91ec-1eba45094043';
}