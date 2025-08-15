@Feature:PostUpdate
@Target:API
@need
Feature: Post Update Operations
    Tests for Post Update operations using ReqRes API

    @PostUpdate
    @need:update
    Scenario Outline: Update post and verify the updated post
        Given the user is logged in with valid credentials
        And a post exists with <postType>
        When the post is updated with <updateType>
        And the updated post is retrieved
        Then the post update is <result>
        And the retrieved post matches the updated data
        Examples:
            | postType | updateType | result       |
            | Valid    | Valid      | Updated      |
            | Valid2   | Valid      | Updated      |

    @PostUpdate
    @need:update
    Scenario Outline: Update post with invalid data (ReqRes API accepts all data)
        Given the user is logged in with valid credentials
        And a post exists with <postType>
        When the post is updated with <updateType>
        Then the post update is <result>
        Examples:
            | postType | updateType | result       |
            | Valid    | Invalid    | Updated      |
            | Valid    | Empty      | Updated      | 