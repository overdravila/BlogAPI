using AutoMapper;
using BlogAPI.Biz.Models;
using BlogAPI.Contracts.Requests;
using BlogAPI.Data.Models;

namespace BlogAPI.Profiles
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<UserRegisterRequest, UserPerson>();
            CreateMap<PostRequest, Post>();
            CreateMap<CommentRequest, Comment>();
            CreateMap<PostRejectionCommentRequest, PostRejectionComment>();
        }
    }
}
