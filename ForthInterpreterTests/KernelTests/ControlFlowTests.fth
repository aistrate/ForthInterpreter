: testIf
    0> ." number is " if ." greater" else ." equal or less" then ."  than zero" ;


: testDiv
    dup 2 mod 0=
    if
        4 mod 0=
        if
            ." div by 4"
        else
            ." div by 2"
        then
    else
        drop
        ." not div by 2"
    then
    cr ;

: testDivLoop
    cr 16 0 
    do
        i . i testDiv
    loop ;


: fizzBuzz
    cr
    101 1
    do
        i 15 mod 0=
        if
            ." fizzbuzz "
        else
            i 3 mod 0=
            if
                ." fizz "
            else
                i 5 mod 0=
                if
                    ." buzz "
                else 
                    i .
                then
            then
        then
        i 10 mod 0=
        if cr
        then
    loop ;


: testExit
    945 exit 721 ;


: testDivExit
    ." number is "
    dup 2 mod 0=
    if
        4 mod 0=
        if
            ." div by 4" exit ."  (four)"
        else
            ." div by 2"
        then
        ."  (end if)"
    else
        drop
        ." not div by 2"
    then
    ."  (end callee)" ;

: testExitCall
    ." result: " testDivExit ."  (end caller)"  ;


: stars
    ." Loop: " 0 
    do [char] * emit space i .
    loop 
    ." end" ;


: loopOnce
    do i . loop ;


: 2loops
    4 0 
    do 
        3 0 
        do j . i . 2 spaces
        loop
    loop ;


: unloopTest
    4 0
    do
        3 0
        do 
            j . i . 
            j 2 =
            if
                unloop unloop exit
            then
            ." * "
        loop
        ." $   "
    loop
    ." unreachable" ;


: starsLeave
    ." Loop: " 0 
    do
        [char] * emit space 
        i 4 = 
        if leave then
        i .
    loop 
    ." end" ;


: starsQuit
    ." Loop: " 0 
    do
        [char] * emit space 
        i 4 = 
        if quit then
        i .
    loop 
    ." end" ;

: starsQuitCall
    ." (before) " starsQuit ."  (after)" ;

: starsQuit2Call
    ." (before 2) " starsQuitCall ."  (after 2)" ;


: starsAbort
    ." Loop: " 0 
    do
        [char] * emit space 
        i 7 = 
        if abort then
        i .
    loop
    ." end" ;


: evenNums
    [char] b emit space 
    0
    do i . 2
    +loop 
    [char] e emit ;


: descLoop
    0 9
    do i . -1
    +loop ;


: zeroIncLoop
    do i . 0
    +loop ;


: spaces'
    0 ?do space loop ;


: qleave
    [char] a emit space
    20 1
    do
        i 7 mod 0= ?leave
        i .
    loop
    [char] z emit ;


: beginAgain
    11 1 do i loop
    begin
        depth 0=
        if exit
        then
        .
    again
    ." never reached" ;


: beginUntil
    15 1 do i loop
    begin
        .
        depth 0=
    until
    ." end" ;


variable fib-0
variable fib-1

: fibonacci
    0 fib-0 !
    1 fib-1 !
    0 .
    begin dup fib-1 @ >=
    while fib-0 @ fib-1 @ + 
          fib-1 @ fib-0 !
          fib-1 !
          fib-0 ?
    repeat
    drop ." end" ;

: fibonacci'
	0 1 0 .
	begin rot 2dup <= >r -rot r>
	while swap over + over .
    repeat
    2drop drop ." end'" ;


: fib-exit1
	0 1 0 .
	begin dup 500 > if 2drop drop exit then
		rot 2dup <= >r -rot r>
	while swap over + over .
    repeat
    2drop drop ." end'" ;

: fib-exit2
	0 1 0 .
	begin rot 2dup <= >r -rot r>
	while dup 800 > if 2drop drop exit then
		swap over + over .
    repeat
    2drop drop ." end'" ;


: style?
    ." : "
    case
        1 of ." Mummy, I like you" endof
        2 of ." Pleased to meet you" endof
        3 of ." Hi!" endof
        4 of ." Hello" endof
        5 of ." Where's the coffee" endof
        6 of ." Yes?" endof
        ." And who are you?"
    endcase 
    cr ;

: teststyles
    cr 9 -1
    do i . i style?
    loop ;


: style-exit
    ." : "
    case
        1 of ." Mummy, I like you" endof
        2 of ." Pleased to meet you" endof
        drop exit
        3 of ." Hi!" endof
        ." And who are you?"
    endcase 
    ."  Bye" ;
