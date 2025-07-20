# 📋 Pull Request

## 🔗 Related Issue
Closes #[issue number]  
Related to #[issue number] (if applicable)

## 📝 Description
<!-- Provide a clear and concise description of what this PR does -->

### Changes Made
- [ ] [Specific change 1]
- [ ] [Specific change 2]
- [ ] [Specific change 3]

### Technical Details
<!-- Include technical implementation details, architecture decisions, etc. -->

## 🧪 Type of Change
- [ ] 🐛 Bug fix (non-breaking change which fixes an issue)
- [ ] ✨ New feature (non-breaking change which adds functionality)
- [ ] 💥 Breaking change (fix or feature that would cause existing functionality to not work as expected)
- [ ] 📚 Documentation update
- [ ] 🔧 Refactoring (no functional changes)
- [ ] ⚡ Performance improvement
- [ ] 🧪 Test updates

## ✅ Checklist

### 📱 Responsive Design Testing
- [ ] Tested on desktop (1024px+)
- [ ] Tested on tablet (768px-1023px)  
- [ ] Tested on mobile (320px-767px)
- [ ] Touch interactions work properly on mobile
- [ ] Text is readable without zooming on mobile
- [ ] Loading states visible on all screen sizes
- [ ] Error messages appropriately sized for all screens

### 🌐 Cross-Browser Testing
- [ ] Chrome (latest)
- [ ] Safari (latest)
- [ ] Firefox (latest)
- [ ] Edge (latest)

### 🔧 Code Quality
- [ ] My code follows the style guidelines of this project
- [ ] I have performed a self-review of my own code
- [ ] I have commented my code, particularly in hard-to-understand areas
- [ ] I have made corresponding changes to the documentation
- [ ] My changes generate no new warnings or errors
- [ ] I have added tests that prove my fix/feature is effective
- [ ] New and existing unit tests pass locally with my changes

### 🧪 Testing
- [ ] Unit tests added/updated and passing
- [ ] Integration tests added/updated and passing
- [ ] Manual testing completed
- [ ] Performance testing completed (if applicable)
- [ ] Accessibility testing completed (basic ARIA compliance)

### 🔐 Security
- [ ] Input validation implemented (frontend and backend)
- [ ] No sensitive data exposed in logs or error messages
- [ ] Authentication/authorization working correctly
- [ ] CORS configuration appropriate
- [ ] No hardcoded secrets or credentials

### 📊 Performance
- [ ] API endpoints respond within 200ms (95th percentile)
- [ ] Frontend pages load within 2 seconds
- [ ] Images optimized and compressed
- [ ] No memory leaks introduced
- [ ] Database queries optimized

### 📚 Documentation
- [ ] README updated (if needed)
- [ ] API documentation updated (if backend changes)
- [ ] Code comments added for complex logic
- [ ] Architecture documentation updated (if applicable)

## 📸 Screenshots/Videos
<!-- Include screenshots for UI changes, videos for complex interactions -->

### Before
<!-- Screenshot of before state (if applicable) -->

### After  
<!-- Screenshot of after state -->

### Mobile View
<!-- Screenshot of mobile responsive view (if applicable) -->

## 🧪 Testing Instructions

### Local Testing
```bash
# Steps to test this PR locally
1. git checkout [branch-name]
2. npm install (if frontend changes)
3. dotnet restore (if backend changes)
4. [specific testing steps]
```

### Manual Test Cases
1. **Test Case 1**: [Description]
   - Steps: [Step 1, Step 2, Step 3]
   - Expected: [Expected result]

2. **Test Case 2**: [Description]
   - Steps: [Step 1, Step 2, Step 3] 
   - Expected: [Expected result]

## 🔄 Deployment Notes
<!-- Any special deployment considerations -->

- [ ] Database migrations included (if applicable)
- [ ] Environment variables updated (if applicable)
- [ ] Feature flags configured (if applicable)
- [ ] Rollback plan documented (if needed)

## ⚠️ Breaking Changes
<!-- List any breaking changes and migration steps -->

- [ ] No breaking changes
- [ ] Breaking changes documented below:

### Migration Steps
<!-- If there are breaking changes, provide migration steps -->

## 📋 Additional Notes
<!-- Any additional information that reviewers should know -->

### Known Issues
<!-- List any known issues that will be addressed in future PRs -->

### Future Improvements
<!-- List any future improvements that could be made -->

## 📝 Review Checklist (for reviewers)
- [ ] Code follows project conventions and standards
- [ ] Logic is clear and well-documented
- [ ] Tests provide adequate coverage
- [ ] Performance impact is acceptable
- [ ] Security considerations addressed
- [ ] Responsive design working correctly
- [ ] No regressions introduced

---

## 🎯 DentalTrack Specific Checks

### 🦷 Medical/Clinical Considerations
- [ ] Medical data handled securely and appropriately
- [ ] HIPAA compliance considerations addressed
- [ ] Patient privacy protected
- [ ] Clinical workflow not disrupted

### 🤖 AI/Analysis Features (if applicable)
- [ ] AI analysis results are accurate and reliable
- [ ] Error handling for AI service failures
- [ ] Performance acceptable for large images
- [ ] Results properly validated before display

### 🔐 Authentication & Authorization
- [ ] Google OAuth flow working correctly
- [ ] JWT tokens handled securely
- [ ] Role-based access control working
- [ ] Session management appropriate

---

**💡 Tip**: For complex PRs, consider breaking them into smaller, focused PRs for easier review.