using ApiTestAutomationProject.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;

namespace ApiTestAutomationProject.TestData
{
    public enum EnumPost
    {
        Valid,
        Valid2,
        Invalid,
        Empty,
        ErrNoTitle,
        ErrEmptyTitle,
        ErrNoBody,
        ErrEmptyBody,
        ErrNoUserId,
        ErrInvalidUserId,
        ErrTooLongTitle,
        ErrSpecialChars
    }

    public class PostTestData
    {
        public Dictionary<EnumPost, CreatePostRequest> CreateData => new()
        {
            [EnumPost.Valid] = CreateValidPost,
            [EnumPost.Valid2] = CreateValidPost2,
            [EnumPost.Invalid] = CreateInvalidPost,
            [EnumPost.Empty] = CreateEmptyPost,
            [EnumPost.ErrNoTitle] = CreateNoTitlePost,
            [EnumPost.ErrEmptyTitle] = CreateEmptyTitlePost,
            [EnumPost.ErrNoBody] = CreateNoBodyPost,
            [EnumPost.ErrEmptyBody] = CreateEmptyBodyPost,
            [EnumPost.ErrNoUserId] = CreateNoUserIdPost,
            [EnumPost.ErrInvalidUserId] = CreateInvalidUserIdPost,
            [EnumPost.ErrTooLongTitle] = CreateTooLongTitlePost,
            [EnumPost.ErrSpecialChars] = CreateSpecialCharsPost,
        };

        public Dictionary<EnumPost, UpdatePostRequest> UpdateData => new()
        {
            [EnumPost.Valid] = CreateValidUpdatePost,
            [EnumPost.Valid2] = CreateValidUpdatePost2,
            [EnumPost.Invalid] = CreateInvalidUpdatePost,
            [EnumPost.Empty] = CreateEmptyUpdatePost,
            [EnumPost.ErrNoTitle] = CreateNoTitleUpdatePost,
            [EnumPost.ErrEmptyTitle] = CreateEmptyTitleUpdatePost,
            [EnumPost.ErrNoBody] = CreateNoBodyUpdatePost,
            [EnumPost.ErrEmptyBody] = CreateEmptyBodyUpdatePost,
            [EnumPost.ErrNoUserId] = CreateNoUserIdUpdatePost,
            [EnumPost.ErrInvalidUserId] = CreateInvalidUserIdUpdatePost,
            [EnumPost.ErrTooLongTitle] = CreateTooLongTitleUpdatePost,
            [EnumPost.ErrSpecialChars] = CreateSpecialCharsUpdatePost,
        };

        // Belirli user ID'leri
        public static int ValidUserId = 1;
        public static int ValidUserId2 = 2;
        public static int InvalidUserId = 999;

        private CreatePostRequest CreateValidPost => new()
        {
            Title = "Test Post Title",
            Body = "This is a test post body content",
            UserId = ValidUserId
        };

        private CreatePostRequest CreateValidPost2 => new()
        {
            Title = "Another Test Post",
            Body = "This is another test post body content",
            UserId = ValidUserId2
        };

        private CreatePostRequest CreateInvalidPost => new()
        {
            Title = "Invalid Post Title",
            Body = "This is an invalid post body content",
            UserId = InvalidUserId
        };

        private CreatePostRequest CreateEmptyPost => new()
        {
            Title = "",
            Body = "",
            UserId = ValidUserId
        };

        private CreatePostRequest CreateNoTitlePost => new()
        {
            Title = null!,
            Body = "This is a test post body content",
            UserId = ValidUserId
        };

        private CreatePostRequest CreateEmptyTitlePost => new()
        {
            Title = "",
            Body = "This is a test post body content",
            UserId = ValidUserId
        };

        private CreatePostRequest CreateNoBodyPost => new()
        {
            Title = "Test Post Title",
            Body = null!,
            UserId = ValidUserId
        };

        private CreatePostRequest CreateEmptyBodyPost => new()
        {
            Title = "Test Post Title",
            Body = "",
            UserId = ValidUserId
        };

        private CreatePostRequest CreateNoUserIdPost => new()
        {
            Title = "Test Post Title",
            Body = "This is a test post body content",
            UserId = 0
        };

        private CreatePostRequest CreateInvalidUserIdPost => new()
        {
            Title = "Test Post Title",
            Body = "This is a test post body content",
            UserId = InvalidUserId
        };

        private CreatePostRequest CreateTooLongTitlePost => new()
        {
            Title = new string('A', 1001),
            Body = "This is a test post body content",
            UserId = ValidUserId
        };

        private CreatePostRequest CreateSpecialCharsPost => new()
        {
            Title = "Test@Post#Title$123",
            Body = "This is a test post body content",
            UserId = ValidUserId
        };

        private UpdatePostRequest CreateValidUpdatePost => new()
        {
            Title = "Updated Test Post Title",
            Body = "This is an updated test post body content",
            UserId = ValidUserId
        };

        private UpdatePostRequest CreateValidUpdatePost2 => new()
        {
            Title = "Updated Another Test Post",
            Body = "This is an updated another test post body content",
            UserId = ValidUserId2
        };

        private UpdatePostRequest CreateInvalidUpdatePost => new()
        {
            Title = "Invalid Updated Post Title",
            Body = "This is an invalid updated post body content",
            UserId = InvalidUserId
        };

        private UpdatePostRequest CreateEmptyUpdatePost => new()
        {
            Title = "",
            Body = "",
            UserId = ValidUserId
        };

        private UpdatePostRequest CreateNoTitleUpdatePost => new()
        {
            Title = null!,
            Body = "This is a test post body content",
            UserId = ValidUserId
        };

        private UpdatePostRequest CreateEmptyTitleUpdatePost => new()
        {
            Title = "",
            Body = "This is a test post body content",
            UserId = ValidUserId
        };

        private UpdatePostRequest CreateNoBodyUpdatePost => new()
        {
            Title = "Test Post Title",
            Body = null!,
            UserId = ValidUserId
        };

        private UpdatePostRequest CreateEmptyBodyUpdatePost => new()
        {
            Title = "Test Post Title",
            Body = "",
            UserId = ValidUserId
        };

        private UpdatePostRequest CreateNoUserIdUpdatePost => new()
        {
            Title = "Test Post Title",
            Body = "This is a test post body content",
            UserId = 0
        };

        private UpdatePostRequest CreateInvalidUserIdUpdatePost => new()
        {
            Title = "Test Post Title",
            Body = "This is a test post body content",
            UserId = InvalidUserId
        };

        private UpdatePostRequest CreateTooLongTitleUpdatePost => new()
        {
            Title = new string('A', 1001),
            Body = "This is a test post body content",
            UserId = ValidUserId
        };

        private UpdatePostRequest CreateSpecialCharsUpdatePost => new()
        {
            Title = "Test@Post#Title$123",
            Body = "This is a test post body content",
            UserId = ValidUserId
        };

        public void Assert(EnumPost expectedPostEnum, CreatePostResponse post)
        {
            post.Should().NotBeNull();
            var expectedPost = CreateData[expectedPostEnum];
            AssertEqual(expectedPost, post);
        }

        public void AssertUpdate(EnumPost expectedPostEnum, UpdatePostResponse post)
        {
            post.Should().NotBeNull();
            var expectedPost = UpdateData[expectedPostEnum];
            AssertEqualUpdate(expectedPost, post);
        }

        private static void AssertEqual(CreatePostRequest expectedPost, CreatePostResponse post)
        {
            post.Title.Should().Be(expectedPost.Title);
            post.Body.Should().Be(expectedPost.Body);
            post.UserId.Should().Be(expectedPost.UserId);
        }

        private static void AssertEqualUpdate(UpdatePostRequest expectedPost, UpdatePostResponse post)
        {
            post.Title.Should().Be(expectedPost.Title);
            post.Body.Should().Be(expectedPost.Body);
            post.UserId.Should().Be(expectedPost.UserId);
        }
    }
} 