	require 'debugger'

	module M
	  def self.included(base)
	    base.class_eval do
	      @inclusion_time = Time.now

		  def included_at
		  	
	        self.class.instance_eval{@inclusion_time}
		  end
		end
	  end
	end

	class A
	  include M
	end

	sleep 3

	class B
	  include M
	end

	sleep 3

	class C
	  include M
	end

	a = A.new
	b = B.new
	c = C.new

	puts a.included_at
	puts b.included_at
	puts c.included_at
