package main

import "fmt"

type int1 interface{
	Func1() int
}

type int2 interface{
	Func2() int
}

type imp1 struct{}

func (i *imp1) Func1() int{
	fmt.Println("implements interface 1")
	return 1
}

func (i *imp1) Func2() int{
	fmt.Println("implements interface 2")
	return 2
}

func tester(i int2){
	fmt.Println("Passed");
}

func main(){
	var str = imp1{}
	tester(&str)
}