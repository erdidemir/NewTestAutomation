@Feature:PostCreate
@Target:API
Feature: Post Create Operations
    Tests for Post Create operations using ReqRes API

    @BI:
    @Submenu:CreateAndGet
    @Subject:Privileges
    @Target:API
    Scenario: Create post and get the created post
        Given the user is logged in with valid credentials
        When a post with <postType> is created
        And the created post is retrieved
        Then the post creation is <result>
        And the retrieved post matches the created post
        Examples:
            | postType | result       |
            | Valid    | Created      |
            | Valid2   | Created      | 