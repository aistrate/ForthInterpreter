: test-array-size
  ." size test: "
  here
  20 array
  here swap - 80 =
  if ." success" 
  else ." error" 
  then ;

20 array aa

: formula
  10 + dup * ;

: test-array
  ." assign-retrieve test: "
  20 0
  do i formula 
	 i aa !
  loop
  0 19
  do i aa @ 
     i formula 
     <>
     if ." error" unloop exit
     then
     -1
  +loop
  ." success" ;


: constant'      \ n -- ; -- n
  create
    ,
  does>
    @ ;

12345 constant' cc

: test-const
  cc 12345 =
  if ." success"
  else ." error"
  then ;
