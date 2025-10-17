## Table of Contents

-   [Contributing to PROJECT-NAME](#contributing-to-project-name)
-   [Branch Naming](#branch-naming-convention)
-   [Pull Requests](#pull-requests)
    -   [PR Title Format](#pr-title-format)
    -   [PR Title Example](#pr-title-example)
    -   [PR Labels](#pr-labels)
    -   [PR Description](#pr-description)

## Contributing to pizzapantry_backend WebAPI

This section outlines guidelines for contributing to pizzapantry_backend WebAPI.

### Main Branches

The main branches of the application are:

<img src="https://img.shields.io/badge/branch-env%2Fproduction-blue" alt=""/>
<img src="https://img.shields.io/badge/branch-env%2Fdevelopment-blue" alt=""/>
<img src="https://img.shields.io/badge/branch-env%2Fstaging-blue" alt=""/>

### Branch Naming Convention

Follow these conventions when naming branches:

1. `feature/branch_name`: When developing a new feature for the application.
2. `fix/branch_name`: When fixing a bug before making a PR to **env/development**.
3. `hot_fix/branch_name`: When addressing emergency bugs for production.
4. `update/branch_name`: When you are making updates on existing code of the application.
5. `testing/branch_name`: When you are testing a feature or an issue.

> **IMPORTANT: Use snake_case for branch names.**

## Pull Requests

Requirements for creating pull requests (PRs) for the application:

### PR Title Format

Ensure PR titles follow this format:

```
<BRANCH-TYPE>: <PR-TITLE>
```

Where:

-   **\<BRANCH-TYPE>**: Corresponds to the branch naming conventions outlined [above](#branch-naming-convention).
-   **\<PR-TITLE>**: Descriptive title for your pull request.

### PR Title Example

```
fix: resolve issue with user authentication
```

> **NOTE: The main reason the title follows this pattern is for any tracking sheets.**

### PR Labels

Allocate the following labels for each PR:

1. **Type of PR** ⇒ `feature`, `fix`, `hot-fix`, `update`, `testing`
2. **Environment** ⇒ `production` or `qa`
3. **Migrations** ⇒ `has-migrations` or `no-migrations`
4. **Story Points Allocation** ⇒ `[1, 2, 3, 5, 8, 13]`
5. **Code Review** ⇒ Use `code-review` for explicit code review requests.

Also, provide the **Assignee** and the **Reviewer** if necessary.

### PR Description

Include **at least** the link to the corresponding Monday task in the PR description. Ensure the task is documented properly.

> **IMPORTANT: PRs failing to adhere to these guidelines will either be closed or requested for revision.**

For the template, visit [readme-template repository](https://github.com/Softserve-Digital-Development/readme-template), and for labels, refer to [this link](https://github.com/Softserve-Digital-Development/readme-template/tree/main/labels).