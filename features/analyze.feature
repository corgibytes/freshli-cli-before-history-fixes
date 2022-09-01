Feature: analyze
    The primary user-facing command. This command will delegate to other freshli activities to accomplish its work. It will manage work queues to enable parallelization.

    ```
    freshli [global options] analyze [command options] <repo url>|<local dir>
    ```

    With no options specified, performs analysis locally, and then sends the results to the Freshli web app so that the results can be viewed at at URL that will be provided in the command output.

    Scenario: Run the analysis locally
        When I run `freshli analyze`
        Then the output should contain:
        """
        https://freshli.app/
        """

    Scenario: Run the analysis with default options.
        When I run `freshli analyze https://github.com/corgibytes/freshli-fixture-ruby-nokotest`
        Then the output should contain:
        """
        https://freshli.app/
        """

    Scenario: Run the analysis with specific git installation.
        When I run `freshli analyze --git-path=git https://github.com/corgibytes/freshli-fixture-ruby-nokotest`
        Then the output should contain:
        """
        https://freshli.app/
        """

    Scenario: Run the analysis for a specific branch.
        When I run `freshli analyze --branch=trunk https://github.com/corgibytes/freshli-fixture-ruby-nokotest`
        Then the output should contain:
        """
        https://freshli.app/
        """

    Scenario: Run the analysis for every point in time when the files have changed.
        When I run `freshli analyze --commit-history https://github.com/corgibytes/freshli-fixture-ruby-nokotest`
        Then the output should contain:
        """
        https://freshli.app/
        """

    Scenario: Run the analysis at a specific interval. In the example: take the last point in time per three months that files have changed.
        When I run `freshli analyze --history-interval=3m https://github.com/corgibytes/freshli-fixture-ruby-nokotest`
        Then the output should contain:
        """
        https://freshli.app/
        """

    Scenario: Run the analysis, start with 6 workers.
        When I run `freshli analyze --workers=6 https://github.com/corgibytes/freshli-fixture-ruby-nokotest`
        Then the output should contain:
        """
        https://freshli.app/
        """
